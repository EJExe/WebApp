using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsSuccessful { get; set; }
        public string TransactionId { get; set; }
    }

    public class CreatePaymentDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; }
    }

    public class UpdatePaymentDto
    {
        public int Id { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
