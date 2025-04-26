using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Entities
{
    public class BodyType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; } // "Седан", "Хэтчбек", "Внедорожник"

        public List<Car> Cars { get; set; }
    }
}
