// IOrderRepository.cs
using ProjectManagement.Domain.Entities;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Interfaces
{
    public interface IOrderRepository // Добавьте public
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task AddOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }
}