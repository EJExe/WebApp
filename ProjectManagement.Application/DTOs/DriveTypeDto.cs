using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class DriveTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class CreateCarDriveTypeDto
    {
        [Required(ErrorMessage = "ID типа привода обязателен")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Название типа привода обязательно")]
        public string Name { get; set; }
    }

    public class UpdateCarDriveTypeDto
    {
        [Required(ErrorMessage = "ID типа привода обязателен")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название типа привода обязательно")]
        public string Name { get; set; }
    }
}
