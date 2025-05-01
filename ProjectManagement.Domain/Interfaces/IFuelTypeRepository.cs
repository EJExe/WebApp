using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Interfaces
{
    public interface IFuelTypeRepository
    {
        Task<IEnumerable<FuelType>> GetAllAsync();
        Task<FuelType> GetByIdAsync(int id);
        Task AddAsync(FuelType fuelType);
        Task UpdateAsync(FuelType fuelType);
        Task DeleteAsync(int id);
    }
}
