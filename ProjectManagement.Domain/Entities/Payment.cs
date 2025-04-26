using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // "Card", "PayPal"
        public DateTime PaymentDate { get; set; }
        public bool IsSuccessful { get; set; }
        public string TransactionId { get; set; }
    }
}
