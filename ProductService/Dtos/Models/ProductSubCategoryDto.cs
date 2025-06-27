namespace ProductService.Dtos.Models;

public class ProductSubCategoryDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductDto Product { get; set; } = null!;
    public int SubCategoryId { get; set; }
    public CategoryDto SubCategory { get; set; } = null!;
}