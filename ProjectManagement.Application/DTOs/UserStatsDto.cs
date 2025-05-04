using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class UserStatsDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalSpent { get; set; }
        public double? AverageRating { get; set; }
    }
}
