// ProductService/Dtos/Responses/ProductResponse.cs
using System.ComponentModel.DataAnnotations;

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
        public ICollection<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
        public ICollection<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
    }
}