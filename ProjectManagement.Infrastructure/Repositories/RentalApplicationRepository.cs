using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Data;
using ProjectManagement.Domain.Interfaces;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class RentalApplicationRepository : IRentalApplicationRepository
    {
        private readonly AppDbContext _context;

        public RentalApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RentalApplication>> GetAllAsync()
            => await _context.RentalApplications
                .Include(a => a.User)
                .Include(a => a.Car)
                .ToListAsync();

        public async Task<RentalApplication> GetByIdAsync(int id)
            => await _context.RentalApplications
                .Include(a => a.User)
                .Include(a => a.Car)
                .FirstOrDefaultAsync(a => a.Id == id);

        public async Task AddAsync(RentalApplication application)
        {
            await _context.RentalApplications.AddAsync(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RentalApplication application)
        {
            _context.Entry(application).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var application = await _context.RentalApplications.FindAsync(id);
            if (application != null)
            {
                _context.RentalApplications.Remove(application);
                await _context.SaveChangesAsync();
            }
        }
    }
}