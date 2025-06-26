using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("product/{productId}/images")]
    public class ProductImageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductImageController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /product/{productId}/images
        [HttpGet]
        public async Task<ActionResult<List<ProductImageDto>>> GetImages(int productId)
        {
            var images = await _context.ProductImages
                .Where(i => i.ProductId == productId)
                .OrderBy(i => i.SortOrder)
                .Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    AltText = i.AltText,
                    SortOrder = i.SortOrder
                })
                .ToListAsync();

            return Ok(images);
        }

        // POST: /product/{productId}/images
        [HttpPost]
        public async Task<ActionResult<ProductImageDto>> CreateImage(int productId, [FromBody] ProductImageDto imageDto)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var image = new ProductImage
            {
                ProductId = productId,
                ImageUrl = imageDto.ImageUrl,
                AltText = imageDto.AltText,
                SortOrder = imageDto.SortOrder
            };

            _context.ProductImages.Add(image);
            await _context.SaveChangesAsync();

            imageDto.Id = image.Id;

            return CreatedAtAction(nameof(GetImages), new { productId }, imageDto);
        }

        // PUT: /product/{productId}/images/{imageId}
        [HttpPut("{imageId:int}")]
        public async Task<IActionResult> UpdateImage(int productId, int imageId, [FromBody] ProductImageDto imageDto)
        {
            if (imageId != imageDto.Id) return BadRequest("ID mismatch");

            var image = await _context.ProductImages
                .FirstOrDefaultAsync(i => i.Id == imageId && i.ProductId == productId);

            if (image == null) return NotFound();

            image.ImageUrl = imageDto.ImageUrl;
            image.AltText = imageDto.AltText;
            image.SortOrder = imageDto.SortOrder;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /product/{productId}/images/{imageId}
        [HttpDelete("{imageId:int}")]
        public async Task<IActionResult> DeleteImage(int productId, int imageId)
        {
            var image = await _context.ProductImages
                .FirstOrDefaultAsync(i => i.Id == imageId && i.ProductId == productId);

            if (image == null) return NotFound();

            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
