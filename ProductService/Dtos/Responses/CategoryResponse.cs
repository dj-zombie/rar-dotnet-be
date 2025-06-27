namespace ProductService.Dtos.Responses;

public class CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public List<CategoryResponse>? SubCategories { get; set; }
    public List<ProductResponse>? Products { get; set; }
    public List<ProductSubCategoryResponse>? SubCategoryLinks { get; set; }
}