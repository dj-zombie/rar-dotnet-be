using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(ProductDto productDto);
        Task UpdateAsync(int id, ProductDto productDto);
        Task DeleteAsync(int id);
        Task<List<ProductVariantDto>> GetVariantsAsync(int productId);
    }
}