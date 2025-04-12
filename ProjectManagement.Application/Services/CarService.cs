using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Services
{
    public class CarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<IEnumerable<CarDto>> GetCarsAsync()
        {
            var cars = await _carRepository.GetCarsAsync();
            return cars.Select(c => new CarDto
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                PricePerDay = c.PricePerDay,
                Type = c.Type,
                ImageUrl = c.ImageUrl
            });
        }

        public async Task<CarDto> GetCarByIdAsync(int id)
        {
            var car = await _carRepository.GetCarByIdAsync(id);
            return new CarDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                PricePerDay = car.PricePerDay,
                Type = car.Type,
                ImageUrl = car.ImageUrl
            };
        }

        public async Task AddCarAsync(CreateCarDto carDto)
        {
            var car = new Car
            {
                Brand = carDto.Brand,
                Model = carDto.Model,
                PricePerDay = carDto.PricePerDay,
                Type = carDto.Type,
                ImageUrl = carDto.ImageUrl
            };
            await _carRepository.AddCarAsync(car);
        }

        public async Task UpdateCarAsync(CreateCarDto carDto)
        {
            var car = await _carRepository.GetCarByIdAsync(carDto.Id);
            if (car != null)
            {
                car.Brand = carDto.Brand;
                car.Model = carDto.Model;
                car.PricePerDay = carDto.PricePerDay;
                car.Type = carDto.Type;
                car.ImageUrl = carDto.ImageUrl;
                await _carRepository.UpdateCarAsync(car);
            }
        }

        public async Task DeleteCarAsync(int id)
        {
            await _carRepository.DeleteCarAsync(id);
        }
    }
}