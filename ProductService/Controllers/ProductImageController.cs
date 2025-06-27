using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dtos.Models;
using ProductService.Models;
using ProductService.Dtos.Requests;
using ProductService.Dtos.Responses;

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
        public async Task<ActionResult<List<ProductImageResponse>>> GetImages(int productId)
        {
            var images = await _context.ProductImages
                .Where(i => i.ProductId == productId)
                .OrderBy(i => i.SortOrder)
                .Select(i => new ProductImageResponse
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
        public async Task<ActionResult<ProductImageResponse>> CreateImage(int productId, [FromBody] CreateProductImageRequest request)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var image = new ProductImage
            {
                ProductId = productId,
                ImageUrl = request.ImageUrl,
                AltText = request.AltText ?? "",
                SortOrder = request.SortOrder,
            };

            _context.ProductImages.Add(image);
            await _context.SaveChangesAsync();

            var response = new ProductImageResponse
            {
                Id = image.Id,
                ProductId = image.ProductId,
                ImageUrl = image.ImageUrl,
                AltText = image.AltText,
                SortOrder = image.SortOrder,
            };

            return CreatedAtAction(nameof(GetImages), new { productId }, response);
        }

        // PUT: /product/{productId}/images/{imageId}
        [HttpPut("{imageId:int}")]
        public async Task<IActionResult> UpdateImage(int productId, int imageId, [FromBody] CreateProductImageRequest request)
        {
            var image = await _context.ProductImages
                .FirstOrDefaultAsync(i => i.Id == imageId && i.ProductId == productId);

            if (image == null) return NotFound();

            image.ImageUrl = request.ImageUrl;
            image.AltText = request.AltText ?? "";
            image.SortOrder = request.SortOrder;

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
