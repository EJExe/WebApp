// OrderService.cs
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces; // Добавьте using
using System.Threading.Tasks;

namespace ProjectManagement.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if (order == null) return null; // Исправление CS0161

            return new OrderDto
            {
                OrderId = order.OrderId,
                StartDate = order.StartDate,
                EndDate = order.EndDate,
                Status = order.Status
            };
        }
    }
}