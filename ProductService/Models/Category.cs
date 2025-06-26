namespace ProductService.Models;

using System.Collections.Generic;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<ProductSubCategory> SubCategoryLinks { get; set; } = new List<ProductSubCategory>();
}
