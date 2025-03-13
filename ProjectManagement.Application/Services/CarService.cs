using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;

namespace ProjectManagement.Application.Services
{
    public class CarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public IEnumerable<Car> GetAllCars()
        {
            return _carRepository.GetAll();
        }

        public Car GetCarById(int id)
        {
            return _carRepository.GetById(id);
        }

        public void AddCar(Car car)
        {
            _carRepository.Add(car);
        }

        public void UpdateCar(Car car)
        {
            _carRepository.Update(car);
        }

        public void DeleteCar(int id)
        {
            _carRepository.Delete(id);
        }
    }
}
