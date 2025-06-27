// Dtos/Responses/ProductImageResponse.cs
using System.ComponentModel.DataAnnotations;
using ProductService.Dtos.Models;
using ProductService.Dtos.Requests;

namespace ProductService.Dtos.Responses
{
    public class ProductImageResponse : CreateProductImageRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}