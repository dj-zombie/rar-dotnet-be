namespace ProductService.Controllers;

using Microsoft.AspNetCore.Mvc;
using ProductService.Services.Interfaces;
using ProductService.Dtos.Requests;
using ProductService.Dtos.Responses;
using ProductService.Dtos.Models;
using ProductService.Exceptions;


[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/Category
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
    {
        var categories = await _categoryService.GetCategoriesAsync();
        return Ok(categories);
    }

    // GET: api/Category/hierarchy
    [HttpGet("hierarchy")]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategoriesWithHierarchy()
    {
        var categories = await _categoryService.GetCategoriesWithHierarchyAsync();
        return Ok(categories);
    }

    // GET: api/Category/root
    [HttpGet("root")]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetRootCategories()
    {
        var categories = await _categoryService.GetRootCategoriesAsync();
        return Ok(categories);
    }

    // GET: api/Category/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponse>> GetCategory(int id)
    {
        try
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    // GET: api/Category/subcategories/5
    [HttpGet("subcategories/{parentId}")]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetSubCategories(int parentId)
    {
        var subcategories = await _categoryService.GetSubCategoriesAsync(parentId);
        return Ok(subcategories);
    }

    [HttpGet("products/{categoryId}")]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsByCategory(int categoryId)
    {
        var products = await _categoryService.GetProductsByCategoryAsync(categoryId);
        return Ok(products);
    }

    // POST: api/Category
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var category = await _categoryService.CreateAsync(request);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    // PUT: api/Category/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
    {
        try
        {
            await _categoryService.UpdateAsync(id, request);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    // DELETE: api/Category/5
    [HttpDelete("{id}")]
    [ActionName("DeleteProduct")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}