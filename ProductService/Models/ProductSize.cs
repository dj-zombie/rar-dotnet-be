namespace ProductService.Models;

public class ProductSize
{
    public int Id { get; set; }
    public string SizeName { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
