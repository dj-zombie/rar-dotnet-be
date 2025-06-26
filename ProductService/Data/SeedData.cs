using Microsoft.Extensions.DependencyInjection;
using ProductService.Data;
using ProductService.Models;

namespace ProductService;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var context = serviceProvider.GetRequiredService<AppDbContext>();

        if (!context.Products.Any())
        {
            var product1 = new Product
            {
                Name = "Zombie Juice",
                Price = 9.99m,
                Variants = new List<ProductVariant>
                {
                    new ProductVariant { Size = "Small", Color = "Red", Stock = 10 },
                    new ProductVariant { Size = "Large", Color = "Green", Stock = 5 }
                }
            };

            context.Products.Add(product1);
            await context.SaveChangesAsync();
        }
    }
}
