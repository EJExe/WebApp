using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Entities
{
    public class FuelType
    {
        public int Id { get; set; }
        public string Name { get; set; } // "Бензин", "Дизель", "Электричество"
        public List<Car> Cars { get; set; }
    }
}
