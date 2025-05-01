using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Infrastructure.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class CarFeatureRepository : ICarFeatureRepository
    {
        private readonly AppDbContext _context;

        public CarFeatureRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarFeature>> GetAllAsync()
            => await _context.CarFeatures.ToListAsync();

        public async Task<CarFeature> GetByIdAsync(int id)
            => await _context.CarFeatures.FindAsync(id);

        public async Task AddAsync(CarFeature feature)
        {
            await _context.CarFeatures.AddAsync(feature);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarFeature feature)
        {
            _context.Entry(feature).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.CarFeatures.FindAsync(id);
            if (entity != null)
            {
                _context.CarFeatures.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
