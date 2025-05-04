// Models/Order.cs

using ProjectManagement.Domain.Entities;
using System.Text.Json.Serialization;

namespace ProjectManagement.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; } // ID пользователя
        public int CarId { get; set; } // ID автомобиля
        public DateTime StartDate { get; set; } // Дата начала аренды
        public DateTime EndDate { get; set; } // Дата окончания
        public decimal TotalCost { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = "Pending"; // Статус заявки
        public virtual User User { get; set; }
        public virtual Car Car { get; set; }

        //[JsonIgnore]
        //public RentalApplication Application { get; set; }
    }
}