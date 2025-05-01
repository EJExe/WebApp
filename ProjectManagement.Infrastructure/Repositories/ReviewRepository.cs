using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Infrastructure.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ReviewRepository> _logger;

        public ReviewRepository(AppDbContext context, ILogger<ReviewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Поиск отзыва с ID: {id}");
            var review = await _context.Reviews
                .Include(r => r.Order)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReviewId == id);

            if (review == null)
                _logger.LogWarning($"Отзыв с ID {id} не найден");
            else
                _logger.LogInformation($"Найден отзыв: {review.ReviewId}");

            return review;
        }


        public async Task<IEnumerable<Review>> GetAllAsync()
            => await _context.Reviews
                .Include(r => r.Order)
                .Include(r => r.User)
                .ToListAsync();

        public async Task<Review> GetByOrderIdAsync(int orderId)
            => await _context.Reviews
                .Include(r => r.Order)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.OrderId == orderId);

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Review review)
        {
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}
