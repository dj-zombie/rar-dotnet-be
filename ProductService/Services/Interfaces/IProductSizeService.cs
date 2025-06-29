namespace ProductService.Services.Interfaces;

using ProductService.Dtos.Requests;
using ProductService.Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductService.Dtos.Models;

public interface IProductSizeService
{
    Task<IEnumerable<ProductSizeDto>> GetAllAsync();
    Task<ProductSizeDto> GetByIdAsync(int id);
    Task<ProductSizeDto> CreateAsync(CreateProductSizeRequest request);
    Task<ProductSizeDto> UpdateAsync(int id, UpdateProductSizeRequest request);
    Task DeleteAsync(int id);
}
