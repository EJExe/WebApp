using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Domain.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ProjectManagement.Domain.Interfaces
{
    public interface ICarRepository
    {
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(int id);
        Task<Car> GetCarByIdAsync(int id);
        Task<Car> GetCarWithDetailsAsync(int id);
        Task<IEnumerable<Car>> GetCarsAsync();
        Task<IEnumerable<Car>> GetCarsWithDetailsAsync();

    }
}
