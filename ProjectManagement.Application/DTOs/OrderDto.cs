using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        public string UserId { get; set; }

        [Required]
        public int CarId { get; set; }

        // Связанные данные
        public CarDto Car { get; set; }
        public UserDto User { get; set; }
        //public ReviewDto Review { get; set; }
    }

    public class CreateOrderDto
    {
        public int OrderId { get; set; } 

        [Required(ErrorMessage = "Дата начала обязательна")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Дата окончания обязательна")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "ID автомобиля обязательно")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "ID пользователя обязательно")]
        public string UserId { get; set; }
    }

    public class UpdateOrderDto
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Статус обязателен")]
        public string Status { get; set; }
    }
}
