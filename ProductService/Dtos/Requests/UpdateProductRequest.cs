using System.ComponentModel.DataAnnotations;
using ProductService.Dtos.Models;

namespace ProductService.Dtos.Requests
{
    public class UpdateProductRequest
    {
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Url]
        public string? MainImageUrl { get; set; }

        public string? CategoryName { get; set; }

        public ICollection<ProductVariantDto>? Variants { get; set; }
        public ICollection<ProductImageDto>? Images { get; set; }

        // Optional fields that can be updated
        public bool? IsActive { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public int? StockQuantity { get; set; }
        public bool? IsFeatured { get; set; }
    }
}