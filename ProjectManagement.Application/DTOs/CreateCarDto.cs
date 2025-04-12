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
        [Required] public string Brand { get; set; }
        [Required] public string Model { get; set; }
        public decimal PricePerDay { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }
    }
}
