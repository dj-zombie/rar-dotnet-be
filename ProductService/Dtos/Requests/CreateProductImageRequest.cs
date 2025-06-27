// Dtos/Requests/CreateProductImageRequest.cs
using System.ComponentModel.DataAnnotations;
using ProductService.Dtos.Models;

namespace ProductService.Dtos.Requests
{
    public class CreateProductImageRequest
    {
        [Required]
        [Url]
        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(200)]
        public string? AltText { get; set; }

        [Range(0, int.MaxValue)]
        public int SortOrder { get; set; }

        public bool IsMainImage { get; set; }
    }
}