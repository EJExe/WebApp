using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class CreateCarDto
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int FuelTypeId { get; set; }
        public int DriveTypeId { get; set; }
        public int CategoryId { get; set; }
        public int BodyTypeId { get; set; }
        public List<int> FeatureIds { get; set; } = new();
        public string Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string Color { get; set; }
        public int Seats { get; set; }
        public decimal PricePerDay { get; set; }
        public string ImageUrl { get; set; }

        public double Latitude { get; set; }  // Широта

        public double Longitude { get; set; } // Долгота
    }

   
}
