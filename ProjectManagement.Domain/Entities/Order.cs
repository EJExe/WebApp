// Models/Order.cs

using ProjectManagement.Domain.Entities;
using System.Text.Json.Serialization;

namespace ProjectManagement.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = "Pending";

        public virtual User User { get; set; }
        public virtual Car Car { get; set; }
    }
}