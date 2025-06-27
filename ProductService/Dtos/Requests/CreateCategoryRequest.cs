namespace ProductService.Dtos.Requests;

public class CreateCategoryRequest
{
    public string Name { get; set; } = null!;
    public int? ParentCategoryId { get; set; }
}