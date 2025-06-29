using ProductService.Dtos.Requests;
using ProductService.Dtos.Responses;

namespace ProductService.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetProductsAsync();
        Task<ProductResponse?> GetByIdAsync(int id);
        Task<ProductResponse> CreateAsync(CreateProductRequest request);
        Task<ProductResponse> UpdateAsync(int id, UpdateProductRequest request);
        Task<ProductResponse> DeleteAsync(int id);
        Task<List<ProductVariantResponse>> GetVariantsAsync(int productId);
    }
}