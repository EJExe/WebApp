// Infrastructure/Repositories/DriveTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class CarDriveTypeRepository : ICarDriveTypeRepository
    {
        private readonly AppDbContext _context;

        public CarDriveTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarDriveType>> GetAllAsync()
            => await _context.DriveTypes.ToListAsync();

        public async Task<CarDriveType> GetByIdAsync(int id)
            => await _context.DriveTypes.FindAsync(id);

        public async Task AddAsync(CarDriveType driveType)
        {
            await _context.DriveTypes.AddAsync(driveType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarDriveType driveType)
        {
            _context.Entry(driveType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.DriveTypes.FindAsync(id);
            if (entity != null)
            {
                _context.DriveTypes.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}