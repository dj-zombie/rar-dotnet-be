using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // GET /product
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAll()
        {
            var products = await _context.Products
                .Include(p => p.Variants)
                .ToListAsync();

            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Variants = p.Variants.Select(v => new VariantDto
                {
                    Id = v.Id,
                    Name = $"{v.Size} {v.Color}".Trim(),
                    AdditionalPrice = 0m // adjust as needed
                }).ToList()
            }).ToList();

            return Ok(productDtos);
        }

        // GET /product/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Variants = product.Variants.Select(v => new VariantDto
                {
                    Id = v.Id,
                    Name = $"{v.Size} {v.Color}".Trim(),
                    AdditionalPrice = 0m
                }).ToList()
            };

            return Ok(productDto);
        }

        // POST /product
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Variants = productDto.Variants.Select(v => new ProductVariant
                {
                    Size = v.Name.Split(' ')[0],
                    Color = v.Name.Split(' ').Skip(1).FirstOrDefault() ?? "",
                    // You might want to add AdditionalPrice logic here if stored separately
                }).ToList()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            productDto.Id = product.Id;
            for (int i = 0; i < product.Variants.Count; i++)
                productDto.Variants[i].Id = product.Variants.ElementAt(i).Id;

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, productDto);
        }

        // PUT /product/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id)
                return BadRequest("ID mismatch");

            var product = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            product.Name = productDto.Name;
            product.Price = productDto.Price;

            // Update variants: simple approach - remove old variants and add new ones
            _context.ProductVariants.RemoveRange(product.Variants);

            product.Variants = productDto.Variants.Select(v => new ProductVariant
            {
                Size = v.Name.Split(' ')[0],
                Color = v.Name.Split(' ').Skip(1).FirstOrDefault() ?? "",
            }).ToList();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE /product/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            _context.ProductVariants.RemoveRange(product.Variants);
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // --- VARIANTS CRUD ---

        // GET /product/{productId}/variants
        [HttpGet("{productId:int}/variants")]
        public async Task<ActionResult<List<VariantDto>>> GetVariants(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return NotFound();

            var variants = product.Variants.Select(v => new VariantDto
            {
                Id = v.Id,
                Name = $"{v.Size} {v.Color}".Trim(),
                AdditionalPrice = 0m
            }).ToList();

            return Ok(variants);
        }

        // GET /product/{productId}/variants/{variantId}
        [HttpGet("{productId:int}/variants/{variantId:int}")]
        public async Task<ActionResult<VariantDto>> GetVariant(int productId, int variantId)
        {
            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.Id == variantId && v.ProductId == productId);

            if (variant == null)
                return NotFound();

            var variantDto = new VariantDto
            {
                Id = variant.Id,
                Name = $"{variant.Size} {variant.Color}".Trim(),
                AdditionalPrice = 0m
            };

            return Ok(variantDto);
        }

        // POST /product/{productId}/variants
        [HttpPost("{productId:int}/variants")]
        public async Task<ActionResult<VariantDto>> CreateVariant(int productId, [FromBody] VariantDto variantDto)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound();

            var variant = new ProductVariant
            {
                ProductId = productId,
                Size = variantDto.Name.Split(' ')[0],
                Color = variantDto.Name.Split(' ').Skip(1).FirstOrDefault() ?? ""
            };

            _context.ProductVariants.Add(variant);
            await _context.SaveChangesAsync();

            variantDto.Id = variant.Id;
            return CreatedAtAction(nameof(GetVariant), new { productId, variantId = variant.Id }, variantDto);
        }

        // PUT /product/{productId}/variants/{variantId}
        [HttpPut("{productId:int}/variants/{variantId:int}")]
        public async Task<IActionResult> UpdateVariant(int productId, int variantId, [FromBody] VariantDto variantDto)
        {
            if (variantId != variantDto.Id)
                return BadRequest("ID mismatch");

            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.Id == variantId && v.ProductId == productId);

            if (variant == null)
                return NotFound();

            variant.Size = variantDto.Name.Split(' ')[0];
            variant.Color = variantDto.Name.Split(' ').Skip(1).FirstOrDefault() ?? "";

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE /product/{productId}/variants/{variantId}
        [HttpDelete("{productId:int}/variants/{variantId:int}")]
        public async Task<IActionResult> DeleteVariant(int productId, int variantId)
        {
            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.Id == variantId && v.ProductId == productId);

            if (variant == null)
                return NotFound();

            _context.ProductVariants.Remove(variant);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
