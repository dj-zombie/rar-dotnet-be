using System.Threading.Tasks;
using ProductService.Dtos.Models;
using ProductService.Dtos.Requests;
using ProductService.Dtos.Responses;

namespace ProductService.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetProductsAsync();
        Task<ProductResponse?> GetByIdAsync(int id);
        Task<ProductResponse> CreateAsync(CreateProductRequest request);
        Task UpdateAsync(int id, UpdateProductRequest request);
        Task DeleteAsync(int id);
        Task<List<ProductVariantDto>> GetVariantsAsync(int productId);
    }
}