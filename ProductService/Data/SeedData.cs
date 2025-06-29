using Microsoft.Extensions.DependencyInjection;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Categories.Any())
            return; // DB has already been seeded

        var category = new Category { Name = "Apparel" };

        var product = new Product
        {
            Name = "Basic Tee",
            Price = 19.99M,
            Description = "A comfortable, plain t-shirt.",
            MainImageUrl = "/images/tee.jpg",
            Category = category,
            Variants = new List<ProductVariant>
            {
                new ProductVariant { Size = "M", Color = "Black", Stock = 20 },
                new ProductVariant { Size = "L", Color = "White", Stock = 15 }
            },
            // Sizes = new List<ProductSize>
            // {
            //     new ProductSize { SizeName = "M" },
            //     new ProductSize { SizeName = "L" }
            // },
            Images = new List<ProductImage>
            {
                new ProductImage { ImageUrl = "/images/tee-front.jpg" },
                new ProductImage { ImageUrl = "/images/tee-back.jpg" }
            }
        };

        context.Categories.Add(category);
        context.Products.Add(product);
        context.SaveChanges();
    }
}
