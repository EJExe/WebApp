using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Application.Services
{
    public class RentalApplicationService
    {
        private readonly AppDbContext _context;
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RentalApplicationService> _logger;

        public RentalApplicationService(
            AppDbContext context,
            ICarRepository carRepository,
            IUserRepository userRepository,
            ILogger<RentalApplicationService> logger)
        {
            _context = context;
            _carRepository = carRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<RentalApplicationDto> CreateApplicationAsync(CreateRentalApplicationDto dto, string userId)
        {
            var car = await _carRepository.GetCarByIdAsync(dto.CarId);
            if (car == null || car.IsLeasingDisabled)
                throw new InvalidOperationException("Car is not available.");

            var conflictingApplications = await _context.RentalApplications
                .Where(a => a.CarId == dto.CarId && a.Status != "Rejected" && a.Status != "Cancelled" &&
                            (dto.StartDate <= a.EndDate && dto.EndDate >= a.StartDate))
                .AnyAsync();

            if (conflictingApplications)
                throw new InvalidOperationException("Car is not available for the selected dates.");

            var application = new RentalApplication
            {
                UserId = userId,
                CarId = dto.CarId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = "Pending",
                Price = CalculatePrice(car, dto.StartDate, dto.EndDate)
            };

            await _context.RentalApplications.AddAsync(application);
            await _context.SaveChangesAsync();

            return MapToDto(application);
        }

        public async Task<RentalApplicationDto> ApproveApplicationAsync(int id, string adminId)
        {
            var application = await _context.RentalApplications
                .Include(a => a.Car)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
                throw new KeyNotFoundException("Application not found.");

            if (application.Status != "Pending")
                throw new InvalidOperationException("Only pending applications can be approved.");

            application.Status = "Approved";
            await _context.SaveChangesAsync();

            return MapToDto(application);
        }

        public async Task<RentalApplicationDto> RejectApplicationAsync(int id, string adminId)
        {
            var application = await _context.RentalApplications
                .Include(a => a.Car)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
                throw new KeyNotFoundException("Application not found.");

            if (application.Status != "Pending")
                throw new InvalidOperationException("Only pending applications can be rejected.");

            application.Status = "Rejected";
            await _context.SaveChangesAsync();

            return MapToDto(application);
        }

        public async Task<IEnumerable<RentalApplicationDto>> GetAllApplicationsAsync()
        {
            var applications = await _context.RentalApplications
                .Include(a => a.Car)
                .Include(a => a.User)
                .ToListAsync();

            return applications.Select(MapToDto);
        }

        public async Task<RentalApplicationDto> GetApplicationByIdAsync(int id)
        {
            var application = await _context.RentalApplications
                .Include(a => a.Car)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
                throw new KeyNotFoundException("Application not found.");

            return MapToDto(application);
        }

        private decimal CalculatePrice(Car car, DateTime startDate, DateTime endDate)
        {
            var days = (endDate - startDate).Days + 1;
            return car.PricePerDay * days;
        }

        private RentalApplicationDto MapToDto(RentalApplication application)
        {
            return new RentalApplicationDto
            {
                Id = application.Id,
                UserId = application.UserId,
                CarId = application.CarId,
                StartDate = application.StartDate,
                EndDate = application.EndDate,
                Status = application.Status,
                Price = application.Price,
                UserName = application.User?.UserName,
                CarName = application.Car != null ? $"{application.Car.Brand?.Name} {application.Car.Model}" : null
            };
        }
    }
}