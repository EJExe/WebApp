// UpdateCarDto.cs
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Application.DTOs
{
    public class UpdateCarDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model is required")]
        public string Model { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal PricePerDay { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; }
    }
}