using System.ComponentModel.DataAnnotations;

namespace ProductService.Dtos.Models;

public class ProductSizeDto : BaseDto
{
    [Required]
    [StringLength(50)]
    public string SizeName { get; set; } = string.Empty;

    // [Required]
    // public int Id { get; set; }
}