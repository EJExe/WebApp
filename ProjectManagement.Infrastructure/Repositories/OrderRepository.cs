// OrderRepository.cs
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces; // Добавьте using
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
            => await _context.Orders
                .Include(o => o.Car)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
            => await _context.Orders
                .Include(o => o.Car)
                .Include(o => o.User)
                .ToListAsync();

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
            => await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Car)
                .ToListAsync();
    }
}