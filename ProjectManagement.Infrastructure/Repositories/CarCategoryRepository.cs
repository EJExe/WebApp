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
    public class CarCategoryRepository : ICarCategoryRepository
    {
        private readonly AppDbContext _context;

        public CarCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarCategory>> GetAllAsync()
            => await _context.CarCategories.ToListAsync();

        public async Task<CarCategory> GetByIdAsync(int id)
            => await _context.CarCategories.FindAsync(id);

        public async Task AddAsync(CarCategory category)
        {
            await _context.CarCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarCategory category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.CarCategories.FindAsync(id);
            if (entity != null)
            {
                _context.CarCategories.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
