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
        public string UserId { get; set; }
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = "Pending";

    }

    public class CreateOrderDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; } = "Pending";
    }

    public class UpdateOrderDto
    {
        public int OrderId { get; set; }
        public bool IsActive { get; set; } 
        public string Status { get; set; }
    }

    public class CreateOrderRequest
    {
        [Required]
        public int CarId { get; set; }

        [Required]
        public string UserId { get; set; }

        public string Status { get; set; } = "Pending"; // Добавляем поле
    }

    public class CancelOrderDto
    {
        public int OrderId { get; set; }

    }

    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string error) => new Result(false, error);
    }
}
