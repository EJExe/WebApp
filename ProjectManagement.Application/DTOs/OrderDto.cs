using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        // Связанные данные
        public CarDto Car { get; set; }
        public UserDto User { get; set; }
        //public ReviewDto Review { get; set; }
    }
}
