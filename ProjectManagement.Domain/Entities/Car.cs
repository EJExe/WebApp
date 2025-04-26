using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectManagement.Domain.Entities
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public int Year { get; set; } // Год выпуска
        public int Mileage { get; set; } // Пробег
        public string Color { get; set; } // Цвет
        public int Seats { get; set; } // Количество мест
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal PricePerDay { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }



        //public int BrandId { get; set; }
        //public Brand Brand { get; set; }

        public int DriveTypeId { get; set; }
        public DriveType DriveType { get; set; }

        public int BodyTypeId { get; set; }
        public BodyType BodyType { get; set; }
        public int CarCategoryId { get; set; }
        public CarCategory Category { get; set; }

        public int FuelTypeId { get; set; }
        public FuelType FuelType { get; set; }

        public List<CarFeature> Features { get; set; }

    }
}