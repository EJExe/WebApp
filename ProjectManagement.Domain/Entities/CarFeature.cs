using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Entities
{
    public class CarFeature
    {
        public int Id { get; set; }
        public string Name { get; set; } // "Кондиционер", "Подогрев сидений", "Автопилот"
        public List<Car> Cars { get; set; }
    }
}
