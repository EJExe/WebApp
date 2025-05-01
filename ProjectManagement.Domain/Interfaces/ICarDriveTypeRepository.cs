using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Interfaces
{
    public interface ICarDriveTypeRepository
    {
        Task<IEnumerable<CarDriveType>> GetAllAsync();
        Task<CarDriveType> GetByIdAsync(int id);
        Task AddAsync(CarDriveType driveType);
        Task UpdateAsync(CarDriveType driveType);
        Task DeleteAsync(int id);
    }
}
