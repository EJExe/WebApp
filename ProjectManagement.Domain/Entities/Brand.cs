using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Entities
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } // "Toyota", "BMW", "Audi"

        [StringLength(100)]
        public string Country { get; set; } // Страна производитель

        [Url]
        public string LogoUrl { get; set; } = " "; // Ссылка на логотип

        public List<Car> Cars { get; set; } // Навигационное свойство
    }
}
