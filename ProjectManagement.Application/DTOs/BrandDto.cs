using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class BrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string LogoUrl { get; set; }
    }

    public class CreateBrandDto
    {
        [Required]
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public string? Country { get; set; }
        public string? LogoUrl { get; set; }
    }

    public class UpdateBrandDto
    {
        [Required]
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public string? Country { get; set; }
        public string? LogoUrl { get; set; }
    }
}
