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
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    MainImageUrl = p.MainImageUrl,
                    CategoryName = p.Category.Name,
                    Variants = p.Variants.Select(v => new ProductVariantDto
                    {
                        Id = v.Id,
                        Name = $"{v.Size} {v.Color}".Trim(),
                        AdditionalPrice = 0m
                    }).ToList(),
                    Images = p.Images
                        .OrderBy(i => i.SortOrder)
                        .Select(i => new ProductImageDto
                        {
                            Id = i.Id,
                            ImageUrl = i.ImageUrl,
                            AltText = i.AltText,
                            SortOrder = i.SortOrder
                        }).ToList()

                })
                .ToListAsync();

            return Ok(products);
        }

        // GET /product/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                MainImageUrl = product.MainImageUrl,
                CategoryName = product.Category?.Name ?? "",
                Variants = product.Variants.Select(v => new ProductVariantDto
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
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == productDto.CategoryName);

            if (category == null)
                return BadRequest("Invalid category name.");

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                MainImageUrl = productDto.MainImageUrl,
                CategoryId = category.Id,
                Variants = productDto.Variants.Select(v => new ProductVariant
                {
                    Size = v.Name.Split(' ')[0],
                    Color = v.Name.Split(' ').Skip(1).FirstOrDefault() ?? ""
                }).ToList(),
                Images = productDto.Images.Select(i => new ProductImage
                {
                    ImageUrl = i.ImageUrl,
                    AltText = i.AltText,
                    SortOrder = i.SortOrder
                }).ToList()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Convert to lists so we can index them
            var savedVariants = product.Variants.ToList();
            var savedImages = product.Images.ToList();

            for (int i = 0; i < savedVariants.Count; i++)
                productDto.Variants[i].Id = savedVariants[i].Id;

            for (int i = 0; i < savedImages.Count; i++)
                productDto.Images[i].Id = savedImages[i].Id;

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, productDto);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id)
                return BadRequest("ID mismatch");

            var product = await _context.Products
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == productDto.CategoryName);

            if (category == null)
                return BadRequest("Invalid category name.");

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.MainImageUrl = productDto.MainImageUrl;
            product.CategoryId = category.Id;

            // ---- Update Variants (remove and re-add is okay here) ----
            _context.ProductVariants.RemoveRange(product.Variants);
            product.Variants = productDto.Variants.Select(v => new ProductVariant
            {
                Size = v.Name.Split(' ')[0],
                Color = v.Name.Split(' ').Skip(1).FirstOrDefault() ?? ""
            }).ToList();

            // ---- Update Images (preserve IDs) ----
            // 1. Remove any images not present in DTO
            var dtoImageIds = productDto.Images.Select(i => i.Id).ToHashSet();
            var imagesToRemove = product.Images.Where(i => !dtoImageIds.Contains(i.Id)).ToList();
            _context.ProductImages.RemoveRange(imagesToRemove);

            // 2. Update or add images
            foreach (var imageDto in productDto.Images)
            {
                var existing = product.Images.FirstOrDefault(i => i.Id == imageDto.Id);

                if (existing != null)
                {
                    // Update existing
                    existing.ImageUrl = imageDto.ImageUrl;
                    existing.AltText = imageDto.AltText;
                    existing.SortOrder = imageDto.SortOrder;
                }
                else
                {
                    // Add new image
                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = imageDto.ImageUrl,
                        AltText = imageDto.AltText,
                        SortOrder = imageDto.SortOrder
                    });
                }
            }

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

        // ---------------- VARIANTS CRUD ----------------

        [HttpGet("{productId:int}/variants")]
        public async Task<ActionResult<List<ProductVariantDto>>> GetVariants(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return NotFound();

            var variants = product.Variants.Select(v => new ProductVariantDto
            {
                Id = v.Id,
                Name = $"{v.Size} {v.Color}".Trim(),
                AdditionalPrice = 0m
            }).ToList();

            return Ok(variants);
        }

        [HttpGet("{productId:int}/variants/{variantId:int}")]
        public async Task<ActionResult<ProductVariantDto>> GetVariant(int productId, int variantId)
        {
            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.Id == variantId && v.ProductId == productId);

            if (variant == null)
                return NotFound();

            return Ok(new ProductVariantDto
            {
                Id = variant.Id,
                Name = $"{variant.Size} {variant.Color}".Trim(),
                AdditionalPrice = 0m
            });
        }

        [HttpPost("{productId:int}/variants")]
        public async Task<ActionResult<ProductVariantDto>> CreateVariant(int productId, [FromBody] ProductVariantDto variantDto)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound();

            var variant = new ProductVariant
            {
                ProductId = productId,
                Size = variantDto.Name,
                Color = variantDto.Name
            };

            _context.ProductVariants.Add(variant);
            await _context.SaveChangesAsync();

            variantDto.Id = variant.Id;
            return CreatedAtAction(nameof(GetVariant), new { productId, variantId = variant.Id }, variantDto);
        }

        [HttpPut("{productId:int}/variants/{variantId:int}")]
        public async Task<IActionResult> UpdateVariant(int productId, int variantId, [FromBody] ProductVariantDto variantDto)
        {
            if (variantId != variantDto.Id)
                return BadRequest("ID mismatch");

            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.Id == variantId && v.ProductId == productId);

            if (variant == null)
                return NotFound();

            variant.Size = variantDto.Name;
            variant.Color = variantDto.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }

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

        // ---------------- IMAGES CRUD ----------------

        [ApiController]
        [Route("products/{productId}/images")]
        public class ProductImagesController : ControllerBase
        {
            private readonly AppDbContext _context;

            public ProductImagesController(AppDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<ProductImageDto>>> GetImages(int productId)
            {
                var images = await _context.ProductImages
                    .Where(img => img.ProductId == productId)
                    .OrderBy(img => img.SortOrder)
                    .Select(img => new ProductImageDto
                    {
                        Id = img.Id,
                        ImageUrl = img.ImageUrl,
                        AltText = img.AltText,
                        SortOrder = img.SortOrder
                    }).ToListAsync();

                return Ok(images);
            }

            [HttpPost]
            public async Task<ActionResult<ProductImageDto>> AddImage(int productId, [FromBody] ProductImageDto imageDto)
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                    return NotFound();

                var image = new ProductImage
                {
                    ImageUrl = imageDto.ImageUrl,
                    AltText = imageDto.AltText,
                    SortOrder = imageDto.SortOrder,
                    ProductId = productId
                };

                _context.ProductImages.Add(image);
                await _context.SaveChangesAsync();

                imageDto.Id = image.Id;
                return CreatedAtAction(nameof(GetImages), new { productId }, imageDto);
            }

            [HttpDelete("{imageId}")]
            public async Task<IActionResult> DeleteImage(int productId, int imageId)
            {
                var image = await _context.ProductImages
                    .FirstOrDefaultAsync(img => img.Id == imageId && img.ProductId == productId);

                if (image == null)
                    return NotFound();

                _context.ProductImages.Remove(image);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            [HttpPut("{imageId}")]
            public async Task<IActionResult> UpdateImage(int productId, int imageId, [FromBody] ProductImageDto imageDto)
            {
                if (imageId != imageDto.Id)
                    return BadRequest("ID mismatch");

                var image = await _context.ProductImages
                    .FirstOrDefaultAsync(img => img.Id == imageId && img.ProductId == productId);

                if (image == null)
                    return NotFound();

                image.ImageUrl = imageDto.ImageUrl;
                image.AltText = imageDto.AltText;
                image.SortOrder = imageDto.SortOrder;

                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

    }
}
