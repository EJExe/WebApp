using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class PopularCarDto
    {
        public int CarId { get; set; }
        public string CarName { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
