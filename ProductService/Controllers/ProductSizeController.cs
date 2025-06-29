namespace ProductService.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductService.Services.Interfaces;
using ProductService.Dtos.Requests;
using ProductService.Dtos.Models;



[ApiController]
[Route("api/[controller]")]
public class ProductSizesController : ControllerBase
{
    private readonly IProductSizeService _productSizeService;

    public ProductSizesController(IProductSizeService productSizeService)
    {
        _productSizeService = productSizeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductSizeDto>>> GetAll()
    {
        var sizes = await _productSizeService.GetAllAsync();
        return Ok(sizes);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductSizeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductSizeDto>> GetById(int id)
    {
        try
        {
            var size = await _productSizeService.GetByIdAsync(id);
            return Ok(size);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductSizeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductSizeDto>> Create([FromBody] CreateProductSizeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var newSize = await _productSizeService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = newSize.Id }, newSize);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProductSizeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductSizeDto>> Update(int id, [FromBody] UpdateProductSizeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedSize = await _productSizeService.UpdateAsync(id, request);
            return Ok(updatedSize);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _productSizeService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}