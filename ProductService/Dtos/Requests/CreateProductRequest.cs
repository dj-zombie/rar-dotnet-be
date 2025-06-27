// ProductService/Dtos/Requests/CreateProductRequest.cs
using System.ComponentModel.DataAnnotations;
using ProductService.Dtos.Models;

namespace ProductService.Dtos.Requests
{
    public class CreateProductRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Url]
        public string MainImageUrl { get; set; } = string.Empty;

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
        public ICollection<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();

    }
}