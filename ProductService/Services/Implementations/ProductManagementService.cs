using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using ProductService.Data;
using ProductService.Dtos.Models;
using ProductService.Dtos.Requests;
using ProductService.Dtos.Responses;
using ProductService.Services.Interfaces;

namespace ProductService.Services.Implementations
{
    public class ProductManagementService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductManagementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .Select(p => new ProductResponse
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

        public async Task<ProductResponse?> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return null;

            return new ProductResponse
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

        public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == request.CategoryName);

            if (category == null)
                throw new InvalidOperationException("Invalid category name.");

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                CategoryId = category.Id,
                Variants = request.Variants.Select(v => new ProductVariant
                {
                    Size = v.Size,
                    Color = v.Color,
                    Stock = v.Stock
                }).ToList(),
                Images = request.Images.Select(i => new ProductImage
                {
                    ImageUrl = i.ImageUrl,
                    AltText = i.AltText,
                    SortOrder = i.SortOrder
                }).ToList()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductResponse
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

        public async Task UpdateAsync(int id, UpdateProductRequest request)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new InvalidOperationException("Product not found");

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == request.CategoryName);

            if (category == null)
                throw new InvalidOperationException("Invalid category name.");

            product.Name = request.Name ?? product.Name;
            product.Price = request.Price ?? product.Price;
            product.Description = request.Description ?? product.Description;
            product.MainImageUrl = request.MainImageUrl ?? product.MainImageUrl;
            product.CategoryId = category.Id;

            // Update variants

            // Update images
            if (request.Images != null)
            {
                var imageIds = request.Images.Where(i => i.Id > 0).Select(i => i.Id).ToHashSet();
                var existingImages = product.Images.Where(i => i.Id > 0).ToList();

                // Remove images that are no longer present
                var imagesToRemove = existingImages.Where(i => !imageIds.Contains(i.Id)).ToList();
                _context.ProductImages.RemoveRange(imagesToRemove);

                // Update existing images
                foreach (var image in request.Images)
                {
                    if (image.Id > 0)
                    {
                        var existingImage = existingImages.FirstOrDefault(i => i.Id == image.Id);
                        if (existingImage != null)
                        {
                            existingImage.ImageUrl = image.ImageUrl;
                            existingImage.AltText = image.AltText;
                            existingImage.SortOrder = image.SortOrder;
                        }
                    }
                    else
                    {
                        product.Images.Add(new ProductImage
                        {
                            ImageUrl = image.ImageUrl,
                            AltText = image.AltText,
                            SortOrder = image.SortOrder,
                        });
                    }
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