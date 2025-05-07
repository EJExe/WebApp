using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Services
{
    public class CarService
    {
        private readonly ICarRepository _carRepository;
        private readonly AppDbContext _context;
        private readonly ILogger<CarService> _logger;

        public CarService(ICarRepository carRepository, AppDbContext context, ILogger<CarService> logger)
        {
            _carRepository = carRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<CarSearchResultDto> GetCarsWithPaginationAsync(int pageNumber, int pageSize)
        {
            try
            {
                var cars = await _context.Cars
                    .AsNoTracking()
                    .OrderBy(c => c.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalCount = await _context.Cars.CountAsync();
                var carDtos = cars.Select(MapToDto).ToList();
                return new CarSearchResultDto { Cars = carDtos, TotalCount = totalCount };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cars with pagination");
                throw;
            }
        }

        public async Task<CarSearchResultDto> SearchCarsAsync(CarFilterDto filter)
        {
            try
            {
                var query = _context.Cars.AsQueryable();
                if (filter.BrandId.HasValue)
                    query = query.Where(c => c.BrandId == filter.BrandId.Value);
                if (filter.CategoryId.HasValue)
                    query = query.Where(c => c.CarCategoryId == filter.CategoryId.Value);
                if (filter.FuelTypeId.HasValue)
                    query = query.Where(c => c.FuelTypeId == filter.FuelTypeId.Value);
                if (filter.DriveTypeId.HasValue)
                    query = query.Where(c => c.DriveTypeId == filter.DriveTypeId.Value);
                if (filter.BodyTypeId.HasValue)
                    query = query.Where(c => c.BodyTypeId == filter.BodyTypeId.Value);
                if (filter.MinPrice.HasValue)
                    query = query.Where(c => c.PricePerDay >= filter.MinPrice.Value);
                if (filter.MaxPrice.HasValue)
                    query = query.Where(c => c.PricePerDay <= filter.MaxPrice.Value);

                var totalCount = await query.CountAsync();
                var cars = await query
                    .AsNoTracking()
                    .OrderBy(c => c.Id)
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();

                var carDtos = cars.Select(MapToDto).ToList();
                return new CarSearchResultDto { Cars = carDtos, TotalCount = totalCount };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching cars");
                throw;
            }
        }

        public async Task<CarDto> GetCarByIdAsync(int id)
        {
            try
            {
                var car = await _context.Cars
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);
                return car == null ? null : MapToDto(car);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving car with ID {Id}", id);
                throw;
            }
        }

        public async Task<CarDto> AddCarAsync(CreateCarDto carDto, IFormFile image)
        {
            try
            {
                _logger.LogInformation("Starting AddCarAsync");

                // Проверка существования связанных данных
                if (!await _context.Brands.AnyAsync(b => b.Id == carDto.BrandId))
                    throw new ArgumentException($"Brand with ID {carDto.BrandId} not found");
                if (!await _context.FuelTypes.AnyAsync(f => f.Id == carDto.FuelTypeId))
                    throw new ArgumentException($"FuelType with ID {carDto.FuelTypeId} not found");
                if (!await _context.DriveTypes.AnyAsync(d => d.Id == carDto.DriveTypeId))
                    throw new ArgumentException($"DriveType with ID {carDto.DriveTypeId} not found");
                if (!await _context.BodyTypes.AnyAsync(b => b.Id == carDto.BodyTypeId))
                    throw new ArgumentException($"BodyType with ID {carDto.BodyTypeId} not found");
                if (!await _context.CarCategories.AnyAsync(c => c.Id == carDto.CategoryId))
                    throw new ArgumentException($"CarCategory with ID {carDto.CategoryId} not found");

                var car = new Car
                {
                    // Id не устанавливается, так как генерируется базой
                    BrandId = carDto.BrandId,
                    Model = carDto.Model ?? string.Empty,
                    Year = carDto.Year,
                    Mileage = carDto.Mileage,
                    Color = carDto.Color ?? string.Empty,
                    Seats = carDto.Seats,
                    PricePerDay = carDto.PricePerDay,
                    Latitude = carDto.Latitude,
                    Longitude = carDto.Longitude,
                    ImagePath = carDto.ImagePath ?? string.Empty,
                    IsLeasingDisabled = carDto.IsLeasingDisabled,
                    FuelTypeId = carDto.FuelTypeId,
                    DriveTypeId = carDto.DriveTypeId,
                    BodyTypeId = carDto.BodyTypeId,
                    CarCategoryId = carDto.CategoryId,
                    Features = new List<CarFeature>()

                };

                // Обработка FeatureIds
                if (carDto.FeatureIds != null && carDto.FeatureIds.Any())
                {
                    if (carDto.FeatureIds.Distinct().Count() != carDto.FeatureIds.Count)
                    {
                        throw new ArgumentException("Duplicate Feature IDs detected");
                    }
                    _logger.LogInformation("Processing FeatureIds: {FeatureIds}", string.Join(",", carDto.FeatureIds));
                    //var features = await _context.CarFeatures
                    //    .Where(f => carDto.FeatureIds.Contains(f.Id))
                    //    .ToListAsync();
                    var parameters = string.Join(",", carDto.FeatureIds.Select((_, i) => $"@p{i}"));
                    var query = $"SELECT * FROM CarFeatures WHERE Id IN ({parameters})";

                    var sqlParams = carDto.FeatureIds
                        .Select((id, i) => new SqlParameter($"@p{i}", id))
                        .ToArray();

                    var features = await _context.CarFeatures
                        .FromSqlRaw(query, sqlParams)
                        .ToListAsync();

                    foreach (var feature in features)
                    {
                        _context.Entry(feature).State = EntityState.Unchanged;
                    }

                    if (features.Count != carDto.FeatureIds.Count)
                    {
                        var missingIds = carDto.FeatureIds.Except(features.Select(f => f.Id)).ToList();
                        throw new ArgumentException($"Features with IDs {string.Join(",", missingIds)} not found");
                    }

                    car.Features = features;
                }
                else
                {
                    _logger.LogInformation("No FeatureIds provided");
                }

                _logger.LogInformation("Adding car to database");
                await _context.Cars.AddAsync(car);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Car added successfully with ID: {Id}", car.Id);
                return MapToDto(car);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding car");
                throw;
            }
        }

        public async Task UpdateCarAsync(int id, UpdateCarDto carDto, IFormFile image)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"Updating car with ID {id}. FeatureIds: {string.Join(",", carDto.FeatureIds ?? new List<int>())}");

                var car = await _context.Cars
                    .Include(c => c.Features)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (car == null)
                {
                    _logger.LogWarning($"Car with ID {id} not found");
                    throw new KeyNotFoundException($"Car with ID {id} not found");
                }

                // Проверка внешних ключей
                if (!await _context.Brands.AnyAsync(b => b.Id == carDto.BrandId))
                    throw new ArgumentException($"Brand with ID {carDto.BrandId} not found");
                if (!await _context.BodyTypes.AnyAsync(bt => bt.Id == carDto.BodyTypeId))
                    throw new ArgumentException($"BodyType with ID {carDto.BodyTypeId} not found");
                if (!await _context.CarCategories.AnyAsync(c => c.Id == carDto.CategoryId))
                    throw new ArgumentException($"Category with ID {carDto.CategoryId} not found");
                if (!await _context.DriveTypes.AnyAsync(dt => dt.Id == carDto.DriveTypeId))
                    throw new ArgumentException($"DriveType with ID {carDto.DriveTypeId} not found");
                if (!await _context.FuelTypes.AnyAsync(ft => ft.Id == carDto.FuelTypeId))
                    throw new ArgumentException($"FuelType with ID {carDto.FuelTypeId} not found");

                // Проверка FeatureIds, если они не пустые
                if (carDto.FeatureIds != null && carDto.FeatureIds.Any())
                {
                    var invalidFeatureIds = carDto.FeatureIds
                        .Where(fid => !_context.CarFeatures.Any(f => f.Id == fid))
                        .ToList();
                    if (invalidFeatureIds.Any())
                    {
                        _logger.LogWarning($"Invalid FeatureIds: {string.Join(",", invalidFeatureIds)}");
                        throw new ArgumentException($"Invalid FeatureIds: {string.Join(",", invalidFeatureIds)}");
                    }
                }

                // Обновление основных полей
                car.BrandId = carDto.BrandId;
                car.Model = carDto.Model;
                car.Year = carDto.Year;
                car.Mileage = carDto.Mileage;
                car.Color = carDto.Color;
                car.Seats = carDto.Seats;
                car.PricePerDay = carDto.PricePerDay;
                car.Latitude = carDto.Latitude;
                car.Longitude = carDto.Longitude;
                car.BodyTypeId = carDto.BodyTypeId;
                car.CarCategoryId = carDto.CategoryId;
                car.DriveTypeId = carDto.DriveTypeId;
                car.FuelTypeId = carDto.FuelTypeId;
                car.IsLeasingDisabled = carDto.IsLeasingDisabled;
                car.ImagePath = carDto.ImagePath;

                var currentFeatures = car.Features.ToList();
                foreach (var feature in currentFeatures)
                {
                    car.Features.Remove(feature);
                }

                // Добавляем новые связи по одному
                if (carDto.FeatureIds != null && carDto.FeatureIds.Any())
                {
                    foreach (var featureId in carDto.FeatureIds)
                    {
                        var feature = await _context.CarFeatures.FindAsync(featureId);
                        if (feature != null)
                        {
                            car.Features.Add(feature);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                _logger.LogInformation($"Car with ID {id} updated successfully");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Database error updating car with ID {id}. InnerException: {ex.InnerException?.Message}");
                throw new Exception("Failed to update car due to a database error", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating car with ID {id}. InnerException: {ex.InnerException?.Message}");
                throw new Exception("Failed to update car", ex);
            }
        }

        public async Task DeleteCarAsync(int id)
        {
            try
            {
                var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
                if (car == null)
                    throw new KeyNotFoundException("Car not found");

                var activeOrders = await _context.Orders
                    .AnyAsync(o => o.CarId == id && o.Status != "Cancelled");
                if (activeOrders)
                    throw new ApplicationException("Cannot delete car with active orders");

                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Car with ID {Id} deleted successfully", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting car with ID {Id}", id);
                throw;
            }
        }

        private CarDto MapToDto(Car car)
        {
            return new CarDto
            {
                Id = car.Id,
                BrandId = car.BrandId,
                BrandName = _context.Brands.FirstOrDefault(b => b.Id == car.BrandId)?.Name,
                Model = car.Model,
                Year = car.Year,
                Mileage = car.Mileage,
                Color = car.Color,
                Seats = car.Seats,
                PricePerDay = car.PricePerDay,
                Latitude = car.Latitude,
                Longitude = car.Longitude,
                ImagePath = car.ImagePath,
                IsAvailable = car.IsLeasingDisabled,
                FuelTypeId = car.FuelTypeId,
                FuelTypeName = _context.FuelTypes.FirstOrDefault(f => f.Id == car.FuelTypeId)?.Name,
                DriveTypeId = car.DriveTypeId,
                DriveTypeName = _context.DriveTypes.FirstOrDefault(d => d.Id == car.DriveTypeId)?.Name,
                BodyTypeId = car.BodyTypeId,
                BodyTypeName = _context.BodyTypes.FirstOrDefault(b => b.Id == car.BodyTypeId)?.Name,
                CategoryId = car.CarCategoryId,
                CategoryName = _context.CarCategories.FirstOrDefault(c => c.Id == car.CarCategoryId)?.Name,
                FeatureIds = car.Features?.Select(f => f.Id).ToList() ?? new List<int>(),
                FeatureNames = car.Features?.Select(f => f.Name).ToList() ?? new List<string>()
            };
        }
    }
}