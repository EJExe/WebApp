using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Entities
{
    public class CarDriveType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; } // "Передний", "Задний", "Полный"

        public List<Car> Cars { get; set; }
    }
}
