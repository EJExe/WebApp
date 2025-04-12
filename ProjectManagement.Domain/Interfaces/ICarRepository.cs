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
        //IEnumerable<Car> GetAll();
        //Car GetById(int id);
        //void Add(Car car);
        //void Update(Car car);
        //void Delete(int id);

        //Task<Car> GetCarByIdAsync(int id);
        //Task<IEnumerable<Car>> GetCarsAsync();
        //Task AddCarAsync(Car car);
        //Task UpdateCarAsync(Car car);
        //Task DeleteCarAsync(int id);

        Task<Car> GetCarByIdAsync(int id);
        Task<IEnumerable<Car>> GetCarsAsync();
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(int id);
    }
}
