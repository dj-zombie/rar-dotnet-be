namespace ProductService.Models;

using System.Collections.Generic;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
    public string MainImageUrl { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductSize> Sizes { get; set; } = new List<ProductSize>();
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public ICollection<ProductSubCategory> SubCategories { get; set; } = new List<ProductSubCategory>();

    public decimal ShippingPrice { get; set; }          // cost for shipping 
    public bool ProductVisible { get; set; }            // whether product is visible on the site
    public int StockLevel { get; set; }                 // total stock available (if at the product level) 
    public string MetaKeywords { get; set; } = string.Empty;     // for SEO 
    public string MetaDescription { get; set; } = string.Empty;  // for SEO 
    public string ProductUrl { get; set; } = string.Empty;       // SEO-friendly URL or slug 
    public string BrandName { get; set; } = string.Empty;        // brand/manufacturer name
}
