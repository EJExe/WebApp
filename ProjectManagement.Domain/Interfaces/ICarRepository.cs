using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Domain.Entities;
using System.Collections.Generic;

namespace ProjectManagement.Domain.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetCarsWithDetailsAsync();
        Task<Car> GetCarWithDetailsAsync(int id);
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(int id);
    }
}
