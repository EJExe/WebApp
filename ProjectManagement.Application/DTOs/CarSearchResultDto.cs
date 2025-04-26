using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class CarSearchResultDto
    {
        public List<CarDto> Cars { get; set; }
        public int TotalCount { get; set; } // Общее количество найденных авто
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
