using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class CarFilterDto
    {
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int? FuelTypeId { get; set; }
        public int? DriveTypeId { get; set; }
        public int? BodyTypeId { get; set; }
        public List<int>? FeatureIds { get; set; }
        public decimal? MinPricePerDay { get; set; }
        public decimal? MaxPricePerDay { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? MaxDistance { get; set; } // Радиус поиска в км
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
    }
}
