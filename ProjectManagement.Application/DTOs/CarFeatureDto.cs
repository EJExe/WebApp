using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class CarFeatureDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateCarFeatureDto
    {
        [Required]
        public int Id { get; set; }
        [Required] public string Name { get; set; }
    }

    public class UpdateCarFeatureDto
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
    }
}
