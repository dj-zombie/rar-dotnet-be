namespace ProductService.Services.Interfaces;
using ProductService.Dtos.Models;
using ProductService.Dtos.Requests;
using ProductService.Dtos.Responses;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<IEnumerable<CategoryDto>> GetCategoriesWithHierarchyAsync();
    Task<CategoryDto> GetByIdAsync(int id);
    Task<CategoryDto> CreateAsync(CreateCategoryRequest request);
    Task UpdateAsync(int id, UpdateCategoryRequest request);
    Task DeleteAsync(int id);
    Task<IEnumerable<CategoryDto>> GetRootCategoriesAsync();
    Task<IEnumerable<CategoryDto>> GetSubCategoriesAsync(int parentId);
    Task<IEnumerable<ProductResponse>> GetProductsByCategoryAsync(int categoryId);
}