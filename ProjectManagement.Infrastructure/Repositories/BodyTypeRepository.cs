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
    public class BodyTypeRepository : IBodyTypeRepository
    {
        private readonly AppDbContext _context;

        public BodyTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BodyType>> GetAllAsync()
            => await _context.BodyTypes.ToListAsync();

        public async Task<BodyType> GetByIdAsync(int id)
            => await _context.BodyTypes.FindAsync(id);

        public async Task AddAsync(BodyType bodyType)
        {
            await _context.BodyTypes.AddAsync(bodyType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BodyType bodyType)
        {
            _context.Entry(bodyType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.BodyTypes.FindAsync(id);
            if (entity != null)
            {
                _context.BodyTypes.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
