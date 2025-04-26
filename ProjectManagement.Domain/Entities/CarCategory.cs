using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Entities
{
    public class CarCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } // "Эконом", "Премиум", "Внедорожник"
        public string Description { get; set; }
        public List<Car> Cars { get; set; }
    }
}
