namespace ProductService.Models;

using System.ComponentModel.DataAnnotations;

public class ProductVariant
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Size { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Color { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}