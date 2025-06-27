namespace ProductService.Dtos.Responses;

public class ProductSubCategoryResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductResponse Product { get; set; } = null!;
    public int SubCategoryId { get; set; }
    public CategoryResponse SubCategory { get; set; } = null!;
}