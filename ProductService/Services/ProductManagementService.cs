using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Services
{
    public class ProductManagementService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductManagementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    MainImageUrl = p.MainImageUrl,
                    CategoryName = p.Category.Name,
                    Variants = p.Variants.Select(v => new ProductVariantDto
                    {
                        Id = v.Id,
                        Name = $"{v.Size} {v.Color}".Trim(),
                        AdditionalPrice = 0m
                    }).ToList(),
                    Images = p.Images
                        .OrderBy(i => i.SortOrder)
                        .Select(i => new ProductImageDto
                        {
                            Id = i.Id,
                            ImageUrl = i.ImageUrl,
                            AltText = i.AltText,
                            SortOrder = i.SortOrder
                        }).ToList()
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                MainImageUrl = product.MainImageUrl,
                CategoryName = product.Category?.Name ?? "",
                Variants = product.Variants.Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    Name = $"{v.Size} {v.Color}".Trim(),
                    AdditionalPrice = 0m
                }).ToList()
            };
        }

        public async Task<ProductDto> CreateAsync(ProductDto productDto)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == productDto.CategoryName);

            if (category == null)
                throw new InvalidOperationException("Invalid category name.");

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                MainImageUrl = productDto.MainImageUrl,
                CategoryId = category.Id,
                Variants = productDto.Variants.Select(v => new ProductVariant
                {
                    Size = v.Name.Split(' ')[0],
                    Color = v.Name.Split(' ').Skip(1).FirstOrDefault() ?? ""
                }).ToList(),
                Images = productDto.Images.Select(i => new ProductImage
                {
                    ImageUrl = i.ImageUrl,
                    AltText = i.AltText,
                    SortOrder = i.SortOrder
                }).ToList()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Convert to lists so we can index them
            var savedVariants = product.Variants.ToList();
            var savedImages = product.Images.ToList();

            for (int i = 0; i < savedVariants.Count; i++)
                productDto.Variants[i].Id = savedVariants[i].Id;

            for (int i = 0; i < savedImages.Count; i++)
                productDto.Images[i].Id = savedImages[i].Id;

            return productDto;
        }

        public async Task UpdateAsync(int id, ProductDto productDto)
        {
            if (id != productDto.Id)
                throw new InvalidOperationException("ID mismatch");

            var product = await _context.Products
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new InvalidOperationException("Product not found");

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == productDto.CategoryName);

            if (category == null)
                throw new InvalidOperationException("Invalid category name.");

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.MainImageUrl = productDto.MainImageUrl;
            product.CategoryId = category.Id;

            // Update variants
            _context.ProductVariants.RemoveRange(product.Variants);
            product.Variants = productDto.Variants.Select(v => new ProductVariant
            {
                Size = v.Name.Split(' ')[0],
                Color = v.Name.Split(' ').Skip(1).FirstOrDefault() ?? ""
            }).ToList();

            // Update images
            var dtoImageIds = productDto.Images.Select(i => i.Id).ToHashSet();
            var imagesToRemove = product.Images.Where(i => !dtoImageIds.Contains(i.Id)).ToList();
            _context.ProductImages.RemoveRange(imagesToRemove);

            foreach (var imageDto in productDto.Images)
            {
                var existing = product.Images.FirstOrDefault(i => i.Id == imageDto.Id);

                if (existing != null)
                {
                    existing.ImageUrl = imageDto.ImageUrl;
                    existing.AltText = imageDto.AltText;
                    existing.SortOrder = imageDto.SortOrder;
                }
                else
                {
                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = imageDto.ImageUrl,
                        AltText = imageDto.AltText,
                        SortOrder = imageDto.SortOrder
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new InvalidOperationException("Product not found");

            _context.ProductVariants.RemoveRange(product.Variants);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductVariantDto>> GetVariantsAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new InvalidOperationException("Product not found");

            return product.Variants.Select(v => new ProductVariantDto
            {
                Id = v.Id,
                Name = $"{v.Size} {v.Color}".Trim(),
                AdditionalPrice = 0m
            }).ToList();
        }
    }
}