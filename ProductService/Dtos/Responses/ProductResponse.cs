// ProductService/Dtos/Responses/ProductResponse.cs
using System.ComponentModel.DataAnnotations;
using ProductService.Dtos.Models;

namespace ProductService.Dtos.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string MainImageUrl { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public ICollection<ProductVariantResponse> Variants { get; set; } = new List<ProductVariantResponse>();
        public ICollection<ProductImageResponse> Images { get; set; } = new List<ProductImageResponse>();
    }
}