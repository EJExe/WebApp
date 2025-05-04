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
    public class ReservationService
    {
        private readonly AppDbContext _context;
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(
            AppDbContext context,
            ICarRepository carRepository,
            IUserRepository userRepository,
            ILogger<ReservationService> logger)
        {
            _context = context;
            _carRepository = carRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto dto, string userId)
        {
            var car = await _carRepository.GetCarByIdAsync(dto.CarId);
            if (car == null || car.IsLeasingDisabled)
                throw new InvalidOperationException("Car is not available.");

            var conflictingReservations = await _context.Reservations
                .Where(r => r.CarId == dto.CarId && r.Status != "Cancelled" &&
                            (dto.StartDate <= r.EndDate && dto.EndDate >= r.StartDate))
                .AnyAsync();

            if (conflictingReservations)
                throw new InvalidOperationException("Car is not available for the selected dates.");

            var reservation = new Reservation
            {
                UserId = userId,
                CarId = dto.CarId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            return MapToDto(reservation);
        }

        public async Task<ReservationDto> GetReservationByIdAsync(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                throw new KeyNotFoundException("Reservation not found.");

            return MapToDto(reservation);
        }

        public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.User)
                .ToListAsync();

            return reservations.Select(MapToDto);
        }

        public async Task CancelReservationAsync(int id, string userId)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                throw new KeyNotFoundException("Reservation not found.");

            if (reservation.UserId != userId)
                throw new UnauthorizedAccessException("You are not authorized to cancel this reservation.");

            if (reservation.Status != "Pending")
                throw new InvalidOperationException("Only pending reservations can be cancelled.");

            reservation.Status = "Cancelled";
            await _context.SaveChangesAsync();
        }

        private ReservationDto MapToDto(Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                CarId = reservation.CarId,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                Status = reservation.Status,
                CreatedAt = reservation.CreatedAt,
                UserName = reservation.User?.UserName,
                CarName = reservation.Car != null ? $"{reservation.Car.Brand?.Name} {reservation.Car.Model}" : null
            };
        }
    }
}