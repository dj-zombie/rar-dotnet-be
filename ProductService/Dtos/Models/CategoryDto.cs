namespace ProductService.Dtos.Models;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? ParentCategoryId { get; set; }
    public CategoryDto? ParentCategory { get; set; }
    public List<CategoryDto>? SubCategories { get; set; }
    public List<ProductDto>? Products { get; set; }
    public List<ProductSubCategoryDto>? SubCategoryLinks { get; set; }
}