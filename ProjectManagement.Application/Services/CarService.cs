using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Services
{

    public class CarService
    {
        private readonly ICarRepository _carRepository;
        private readonly AppDbContext _context;
        private readonly ILogger<CarService> _logger;

        public CarService(ICarRepository carRepository, AppDbContext context,
            ILogger<CarService> logger)
        {
            _carRepository = carRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<CarDto>> GetCarsAsync()
        {
            var cars = await _carRepository.GetCarsWithDetailsAsync();
            return cars.Select(MapToDto);
        }

        public async Task<CarDto> GetCarByIdAsync(int id)
        {
            var car = await _carRepository.GetCarWithDetailsAsync(id);
            return car == null ? null : MapToDto(car);
        }

        public async Task AddCarAsync(CreateCarDto carDto)
        {
            var car = new Car
            {
                BrandId = carDto.BrandId,
                FuelTypeId = carDto.FuelTypeId,
                DriveTypeId = carDto.DriveTypeId,
                CarCategoryId = carDto.CategoryId,
                BodyTypeId = carDto.BodyTypeId,
                Model = carDto.Model,
                Year = carDto.Year,
                Mileage = carDto.Mileage,
                Color = carDto.Color,
                Seats = carDto.Seats,
                PricePerDay = carDto.PricePerDay,
                ImageUrl = carDto.ImageUrl,
                Latitude = carDto.Latitude,
                Longitude = carDto.Longitude
            };

            // Добавляем машину в контекст (но пока не сохраняем)
            await _context.Cars.AddAsync(car);

            // Устанавливаем связи с характеристиками
            foreach (var featureId in carDto.FeatureIds)
            {
                var feature = await _context.CarFeatures.FindAsync(featureId);
                if (feature != null)
                {
                    car.Features.Add(feature);
                }
            }

            // Сохраняем все изменения одной транзакцией
            await _context.SaveChangesAsync();
        }

        public async Task<CarSearchResultDto> GetCarsWithPaginationAsync(int pageNumber, int pageSize)
        {
            var query = _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .Include(c => c.FuelType)
                .Include(c => c.DriveType)
                .Include(c => c.BodyType)
                .Include(c => c.Features)
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var cars = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => MapToDto(c)) // Используем статический метод
                .ToListAsync();

            return new CarSearchResultDto
            {
                Cars = cars,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task UpdateCarAsync(int id, UpdateCarDto carDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Получаем автомобиль с отслеживанием изменений
                var car = await _context.Cars
                    .Include(c => c.Features)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (car == null) throw new KeyNotFoundException($"Car {id} not found");

                // 2. Валидация связанных сущностей
                var invalidFeatures = carDto.FeatureIds
                    .Except(await _context.CarFeatures.Select(f => f.Id).ToListAsync())
                    .ToList();

                if (invalidFeatures.Any())
                {
                    throw new ArgumentException($"Invalid feature IDs: {string.Join(", ", invalidFeatures)}");
                }

                // 3. Основное обновление
                _context.Entry(car).CurrentValues.SetValues(carDto);

                // 4. Работа с Features через HashSet
                var newFeatures = new HashSet<int>(carDto.FeatureIds);
                var currentFeatures = new HashSet<int>(car.Features.Select(f => f.Id));

                // Удаляем отсутствующие
                foreach (var feature in car.Features.ToList())
                {
                    if (!newFeatures.Contains(feature.Id))
                    {
                        car.Features.Remove(feature);
                    }
                }

                // Добавляем новые
                foreach (var featureId in newFeatures.Where(id => !currentFeatures.Contains(id)))
                {
                    var feature = await _context.CarFeatures.FindAsync(featureId);
                    if (feature != null) car.Features.Add(feature);
                }

                // 5. Сохранение
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error updating car");
                throw new ApplicationException("Update failed. See logs for details.", ex);
            }
        }

        public async Task DeleteCarAsync(int id)
        {
            await _carRepository.DeleteCarAsync(id);
        }

        private static CarDto MapToDto(Car car)
        {
            return new CarDto
            {
                Id = car.Id,
                BrandId = car.BrandId,
                BrandName = car.Brand?.Name,
                FuelTypeId = car.FuelTypeId,
                FuelTypeName = car.FuelType?.Name,
                DriveTypeId = car.DriveTypeId,
                DriveTypeName = car.DriveType?.Name,
                CategoryId = car.CarCategoryId,
                CategoryName = car.Category?.Name,
                Latitude = car.Latitude,
                Longitude = car.Longitude,
                BodyTypeId = car.BodyTypeId,
                BodyTypeName = car.BodyType?.Name,
                FeatureIds = car.Features?.Select(f => f.Id).ToList() ?? new(),
                FeatureNames = car.Features?.Select(f => f.Name).ToList() ?? new(),
                Model = car.Model,
                Year = car.Year,
                Mileage = car.Mileage,
                Color = car.Color,
                Seats = car.Seats,
                PricePerDay = car.PricePerDay,
                ImageUrl = car.ImageUrl
            };
        }
    }
}