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

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return null;

            return new OrderDto
            {
                OrderId = order.OrderId,
                StartDate = order.StartDate,
                EndDate = order.EndDate,
                Status = order.Status,
                // Добавьте CarId и UserId
                CarId = order.CarId,
                UserId = order.UserId,
                Car = order.Car != null ? MapToCarDto(order.Car) : null,
                User = order.User != null ? MapToUserDto(order.User) : null
            };
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersAsync();
                return orders.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка заказов");
                throw;
            }
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            var order = new Order
            {
                CarId = orderDto.CarId,
                UserId = orderDto.UserId,
                StartDate = orderDto.StartDate,
                EndDate = orderDto.EndDate,
                Status = "Pending"
            };

            await _orderRepository.AddOrderAsync(order);

            // Явная загрузка связанных данных
            var createdOrder = await _context.Orders
                .Include(o => o.Car)
                    .ThenInclude(c => c.Brand)
                .Include(o => o.Car)
                    .ThenInclude(c => c.Features)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            return MapToDto(createdOrder); // Ваш метод маппинга с заполнением car и user
        }


        public async Task UpdateOrderAsync(OrderDto orderDto)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderDto.OrderId);
            order.Status = orderDto.Status;
            order.StartDate = orderDto.StartDate;
            order.EndDate = orderDto.EndDate;

            await _orderRepository.UpdateOrderAsync(order);
        }

        private CarDto MapToCarDto(Car car)
        {
            return new CarDto
            {
                Id = car.Id,
                Model = car.Model,
                Year = car.Year,
            };
        }

        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        private OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                StartDate = order.StartDate,
                EndDate = order.EndDate,
                Status = order.Status,
                UserId = order.UserId,
                CarId = order.CarId,
                // Добавьте маппинг связанных сущностей
                Car = order.Car != null ? new CarDto
                {
                    Id = order.Car.Id,
                    Model = order.Car.Model,
                    // ... остальные поля
                } : null,
                User = order.User != null ? new UserDto
                {
                    Id = order.User.Id,
                    UserName = order.User.UserName,
                    Email = order.User.Email
                } : null
            };
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }
    }
}