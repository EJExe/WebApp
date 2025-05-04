using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly AppDbContext _context;

        public CarRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task<Car> GetCarWithDetailsAsync(int id)
        {
            return await _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.FuelType)
                .Include(c => c.DriveType)
                .Include(c => c.Category)
                .Include(c => c.BodyType)
                .Include(c => c.Features)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Car>> GetCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsWithDetailsAsync()
        {
            return await _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.FuelType)
                .Include(c => c.DriveType)
                .Include(c => c.Category)
                .Include(c => c.BodyType)
                .Include(c => c.Features)
                .ToListAsync();
        }

        public async Task AddCarAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCarAsync(Car car)
        {
            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }
    }
}