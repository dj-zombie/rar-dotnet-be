// ProductService/Dtos/Models/ProductVariantDto.cs
using System.ComponentModel.DataAnnotations;

namespace ProductService.Dtos.Models
{
    public class ProductVariantDto : BaseDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal AdditionalPrice { get; set; }
    }
}