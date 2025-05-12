using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Infrastructure.Data;

namespace Lising.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    [Authorize(Roles = "admin")]
    public class StatisticsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatisticsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("revenue")]
        public async Task<ActionResult<decimal>> GetRevenue(DateTime start, DateTime end)
        {
            try
            {
                return await _context.Payments
                    .Where(p => p.PaymentDate >= start && p.PaymentDate <= end)
                    .SumAsync(p => p.Amount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        //[Authorize(Roles = "admin")]
        //[HttpGet("popular-cars")]
        //public async Task<ActionResult<IEnumerable<PopularCarDto>>> GetPopularCars(DateTime start, DateTime end)
        //{
        //    try
        //    {
        //        var popularCars = await _context.Orders
        //            .Where(o => o.StartDate >= start && o.EndDate <= end && o.Status != "Cancelled")
        //            .GroupBy(o => o.Car)
        //            .Select(g => new PopularCarDto
        //            {
        //                CarId = g.Key.Id,
        //                CarName = $"{g.Key.Brand.Name} {g.Key.Model}",
        //                OrderCount = g.Count(),
        //                TotalRevenue = g.Sum(o => o.TotalCost)
        //            })
        //            .OrderByDescending(p => p.OrderCount)
        //            .Take(10)
        //            .ToListAsync();

        //        return Ok(popularCars);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[Authorize(Roles = "admin")]
        //[HttpGet("user-stats")]
        //public async Task<ActionResult<IEnumerable<UserStatsDto>>> GetUserStats(DateTime start, DateTime end)
        //{
        //    try
        //    {
        //        var userStats = await _context.Users
        //            .Select(u => new UserStatsDto
        //            {
        //                UserId = u.Id,
        //                UserName = u.UserName,
        //                OrderCount = u.Orders.Count(o => o.StartDate >= start && o.EndDate <= end && o.Status != "Cancelled"),
        //                TotalSpent = u.Orders
        //                    .Where(o => o.StartDate >= start && o.EndDate <= end && o.Status != "Cancelled")
        //                    .Sum(o => o.TotalCost),
        //                AverageRating = u.Reviews.Any() ? u.Reviews.Average(r => r.Rating) : null
        //            })
        //            .ToListAsync();

        //        return Ok(userStats);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
    }
}
