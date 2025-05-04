using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProjectManagement.Application.DTOs
{
    public class CarDto
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelTypeName { get; set; }
        public int DriveTypeId { get; set; }
        public string DriveTypeName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int BodyTypeId { get; set; }
        public string BodyTypeName { get; set; }
        public List<int> FeatureIds { get; set; } = new();
        public List<string> FeatureNames { get; set; } = new();
        public string Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string Color { get; set; }
        public int Seats { get; set; }
        public decimal PricePerDay { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImagePath { get; set; } = "/images/cars/default.jpg";
        public bool IsAvailable { get; set; }
    }
}