using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;

namespace ProjectManagement.Application.DTOs
{
    public class FuelTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateFuelTypeDto
    {
        [Required(ErrorMessage = "ID типа топлива обязателен")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название типа топлива обязательно")]
        public string Name { get; set; }
    }

    public class UpdateFuelTypeDto
    {
        [Required(ErrorMessage = "ID типа топлива обязателен")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название типа топлива обязательно")]
        public string Name { get; set; }
    }
}
