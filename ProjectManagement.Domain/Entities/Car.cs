using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectManagement.Domain.Entities
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int FuelTypeId { get; set; }
        public FuelType FuelType { get; set; }

        public int DriveTypeId { get; set; }
        public CarDriveType DriveType { get; set; }

        public int CarCategoryId { get; set; }
        public CarCategory Category { get; set; }

        public int BodyTypeId { get; set; }
        public BodyType BodyType { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public int Mileage { get; set; }

        public string Color { get; set; }

        public int Seats { get; set; }

        public decimal PricePerDay { get; set; }

        public string ImageUrl { get; set; }

        public double Latitude { get; set; }  // Широта

        public double Longitude { get; set; } // Долгота

        public List<CarFeature> Features { get; set; } = new();

    }
}