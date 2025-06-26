// ProductService/Dtos/Models/ProductImageDto.cs
using System.ComponentModel.DataAnnotations;

namespace ProductService.Dtos.Models
{
    public class ProductImageDto : BaseDto
    {
        [Required]
        [Url]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string AltText { get; set; } = string.Empty;

        [Required]
        public int SortOrder { get; set; }
    }
}