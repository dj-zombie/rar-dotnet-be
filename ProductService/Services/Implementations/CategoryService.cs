namespace ProductService.Services;

using Microsoft.EntityFrameworkCore;
using ProductService.Dtos.Requests;
using ProductService.Dtos.Models;
using ProductService.Models;
using ProductService.Services.Interfaces;
using ProductService.Data;
using ProductService.Exceptions;
using ProductService.Dtos.Responses;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _context.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategory = c.ParentCategory != null 
                    ? new CategoryDto 
                    { 
                        Id = c.ParentCategory.Id, 
                        Name = c.ParentCategory.Name 
                    } 
                    : null
            })
            .ToListAsync();

        return categories;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesWithHierarchyAsync()
    {
        var categories = await _context.Categories
            .Include(c => c.ParentCategory)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategory = c.ParentCategory != null 
                    ? new CategoryDto 
                    { 
                        Id = c.ParentCategory.Id, 
                        Name = c.ParentCategory.Name 
                    } 
                    : null
            })
            .ToListAsync();

        return categories;
    }

    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.ParentCategory)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategory = c.ParentCategory != null 
                    ? new CategoryDto 
                    { 
                        Id = c.ParentCategory.Id, 
                        Name = c.ParentCategory.Name 
                    } 
                    : null
            })
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            throw new NotFoundException("Category not found");

        return category;
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name,
            ParentCategoryId = request.ParentCategoryId
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
            ParentCategory = category.ParentCategory != null 
                ? new CategoryDto 
                { 
                    Id = category.ParentCategory.Id, 
                    Name = category.ParentCategory.Name 
                } 
                : null
        };
    }

    public async Task UpdateAsync(int id, UpdateCategoryRequest request)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            throw new NotFoundException("Category not found");

        if (request.Name != null) category.Name = request.Name;
        category.ParentCategoryId = request.ParentCategoryId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            throw new NotFoundException("Category not found");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CategoryDto>> GetRootCategoriesAsync()
    {
        var categories = await _context.Categories
            .Where(c => c.ParentCategoryId == null)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();

        return categories;
    }

    public async Task<IEnumerable<CategoryDto>> GetSubCategoriesAsync(int parentId)
    {
        var categories = await _context.Categories
            .Where(c => c.ParentCategoryId == parentId)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();

        return categories;
    }

    public async Task<IEnumerable<ProductResponse>> GetProductsByCategoryAsync(int categoryId)
    {
        var products = await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                MainImageUrl = p.MainImageUrl,
                CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                Variants = p.Variants.Select(v => new ProductVariantResponse
                {
                    Id = v.Id,
                    Size = v.Size,
                    Color = v.Color,
                    Stock = v.Stock
                }).ToList(),
                Images = p.Images.Select(i => new ProductImageResponse
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ImageUrl = i.ImageUrl,
                    AltText = i.AltText,
                    SortOrder = i.SortOrder
                }).ToList()
            })
            .ToListAsync();

        return products;
    }
}