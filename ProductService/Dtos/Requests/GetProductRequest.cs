// Dtos/Requests/GetProductRequest.cs
using System.ComponentModel.DataAnnotations;

namespace ProductService.Dtos.Requests
{
    public class GetProductRequest
    {
        [Range(1, int.MaxValue)]
        public int? Id { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaxPrice { get; set; }

        [StringLength(100)]
        public string? CategoryName { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsFeatured { get; set; }
    }
}