// RentalApplicationDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Application.DTOs
{
    public class RentalApplicationDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public string UserName { get; set; }
        public string CarName { get; set; }
    }

    public class CreateRentalApplicationDto
    {
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}