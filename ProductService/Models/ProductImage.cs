namespace ProductService.Models;

public class ProductImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string AltText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
