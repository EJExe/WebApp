using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;

namespace ProjectManagement.Application.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task ProcessPaymentAsync(CreatePaymentDto dto)
        {
            var order = await _orderRepository.GetOrderByIdAsync(dto.OrderId);
            if (order == null)
                throw new KeyNotFoundException("Order not found");

            var payment = new Payment
            {
                OrderId = dto.OrderId,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                PaymentDate = DateTime.UtcNow,
                IsSuccessful = true,
                TransactionId = Guid.NewGuid().ToString()
            };

            await _paymentRepository.AddAsync(payment);
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return MapToDto(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return payments.Select(MapToDto);
        }

        public async Task UpdatePaymentStatusAsync(UpdatePaymentDto dto)
        {
            var payment = await _paymentRepository.GetByIdAsync(dto.Id);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found");

            payment.IsSuccessful = dto.IsSuccessful;
            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task DeletePaymentAsync(int id)
        {
            await _paymentRepository.DeleteAsync(id);
        }

        private PaymentDto MapToDto(Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                PaymentDate = payment.PaymentDate,
                IsSuccessful = payment.IsSuccessful,
                TransactionId = payment.TransactionId
            };
        }
    }
}
