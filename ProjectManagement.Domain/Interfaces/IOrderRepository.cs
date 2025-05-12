// IOrderRepository.cs
using ProjectManagement.Domain.Entities;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Interfaces
{
    public interface IOrderRepository // Добавьте public
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
    }
}