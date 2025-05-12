// OrderService.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces; // Добавьте using
using ProjectManagement.Infrastructure.Data;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICarRepository _carRepository;
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository,
            ICarRepository carRepository, AppDbContext context,
            IUserRepository userRepository,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _carRepository = carRepository;
            _userRepository = userRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            var orders = await _orderRepository.GetOrdersAsync();
            return orders.Where(o => o.IsActive).Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                CarId = o.CarId,
                StartDate = o.StartDate,
                IsActive = o.IsActive,
                Status = o.Status
            });
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            return order == null ? null : new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                CarId = order.CarId,
                StartDate = order.StartDate,
                IsActive = order.IsActive,
                Status = order.Status
            };
        }

        public async Task AddOrderAsync(OrderDto orderDto)
        {
            var order = new Order
            {
                UserId = orderDto.UserId,
                CarId = orderDto.CarId,
                StartDate = orderDto.StartDate,
                IsActive = orderDto.IsActive,
                Status = "Pending"
            };
            await _orderRepository.AddOrderAsync(order);
            orderDto.OrderId = order.OrderId;
        }

        public async Task<Result> ConfirmOrder(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return Result.Failure("Заказ не найден");

            if (order.Status != "Pending")
                return Result.Failure("Невозможно подтвердить заказ в текущем статусе");

            order.Status = "Confirmed";
            await _orderRepository.UpdateOrderAsync(order);
            return Result.Success();
        }

        public async Task<Result> CancelOrder(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return Result.Failure("Заказ не найден");

            if (order.Status != "Pending")
                return Result.Failure("Невозможно отменить заказ в текущем статусе");

            order.Status = "Cancelled";
            order.IsActive = false;
            await _orderRepository.UpdateOrderAsync(order);
            return Result.Success();
        }

        public async Task<Result> CompleteOrder(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId); 
            if (order == null) return Result.Failure("Заказ не найден");


            order.IsActive = false;
            order.Status = "Completed";

            await _orderRepository.UpdateOrderAsync(order); 

            return Result.Success();
        }


        public async Task UpdateOrderAsync(OrderDto orderDto)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderDto.OrderId);
            if (order != null)
            {
                order.UserId = orderDto.UserId;
                order.CarId = orderDto.CarId;
                order.StartDate = orderDto.StartDate;
                order.IsActive = orderDto.IsActive;
                order.Status = orderDto.Status;
                await _orderRepository.UpdateOrderAsync(order);
            }
        }
    }
} 