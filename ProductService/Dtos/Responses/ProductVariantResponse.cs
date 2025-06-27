namespace ProductService.Dtos.Responses;

public class ProductVariantResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Size { get; set; } = null!;
    public string Color { get; set; } = null!;
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string Name { get; set; } = null!;
    public decimal AdditionalPrice { get; set; }
}