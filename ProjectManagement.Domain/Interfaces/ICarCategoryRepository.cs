using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Interfaces
{
    public interface ICarCategoryRepository
    {
        Task<IEnumerable<CarCategory>> GetAllAsync();
        Task<CarCategory> GetByIdAsync(int id);
        Task AddAsync(CarCategory category);
        Task UpdateAsync(CarCategory category);
        Task DeleteAsync(int id);
    }
}
