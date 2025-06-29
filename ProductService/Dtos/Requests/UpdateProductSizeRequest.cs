namespace ProductService.Dtos.Requests;

public class UpdateProductSizeRequest
{
    public string SizeName { get; set; } = null!;
    public int Id { get; set; }
}
