// ProductService/Dtos/Models/BaseDto.cs
using System.ComponentModel.DataAnnotations;

namespace ProductService.Dtos.Models
{
    public abstract class BaseDto
    {
        [Required]
        public int Id { get; set; }
    }
}