using Microsoft.AspNetCore.Mvc;
using ProductService.Services;
using ProductService.Dtos;
using System.Threading.Tasks;
using ProductService.Services.Interfaces;
using ProductService.Dtos.Models;
using ProductService.Dtos.Requests;
using ProductService.Dtos.Responses;
namespace ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        // GET /product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        // GET /product/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        // POST /product
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request)
        {
            var createdProduct = await _productService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        // PUT /product/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
        {
            try
            {
                await _productService.UpdateAsync(id, request);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
        }

        // DELETE /product/{id}
        [HttpDelete("DeleteById{id:int}")]
        public async Task<ActionResult<ProductResponse>> Delete(int id)
        {
            try
            {
                var deletedProduct = await _productService.DeleteAsync(id);
                return Ok(deletedProduct);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        // GET /product/{productId}/variants
        [HttpGet("{productId:int}/variants")]
        public async Task<ActionResult<List<ProductVariantDto>>> GetVariants(int productId)
        {
            try
            {
                var variants = await _productService.GetVariantsAsync(productId);
                return Ok(variants);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}