using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class FuelTypeRepository : IFuelTypeRepository
    {
        private readonly AppDbContext _context;

        public FuelTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FuelType>> GetAllAsync()
            => await _context.FuelTypes.ToListAsync();

        public async Task<FuelType> GetByIdAsync(int id)
            => await _context.FuelTypes.FindAsync(id);

        public async Task AddAsync(FuelType fuelType)
        {
            await _context.FuelTypes.AddAsync(fuelType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FuelType fuelType)
        {
            _context.Entry(fuelType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.FuelTypes.FindAsync(id);
            if (entity != null)
            {
                _context.FuelTypes.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
