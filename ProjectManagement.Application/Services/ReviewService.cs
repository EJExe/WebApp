using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;

namespace ProjectManagement.Application.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return reviews.Select(r => new ReviewDto
            {
                Id = r.ReviewId,
                Rating = r.Rating,
                Comment = r.Comment,
                OrderId = r.OrderId,
                UserId = r.UserId
            });
        }

        public async Task<ReviewDto> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null) return null;

            return new ReviewDto
            {
                Id = review.ReviewId, 
                Rating = review.Rating,
                Comment = review.Comment,
                OrderId = review.OrderId,
                UserId = review.UserId
            };
        }


        public async Task<ReviewDto> GetReviewByOrderIdAsync(int orderId)
        {
            var review = await _reviewRepository.GetByOrderIdAsync(orderId);
            if (review == null) return null;

            return new ReviewDto
            {
                Id = review.ReviewId,
                Rating = review.Rating,
                Comment = review.Comment,
                OrderId = review.OrderId,
                UserId = review.UserId
            };
        }

        public async Task CreateReviewAsync(ReviewDto reviewDto)
        {
            var review = new Review
            {
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                OrderId = reviewDto.OrderId,
                UserId = reviewDto.UserId
            };
            await _reviewRepository.AddAsync(review);
        }

        public async Task UpdateReviewAsync(ReviewDto reviewDto)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewDto.Id);
            if (review == null)
                throw new KeyNotFoundException("Review not found");

            review.Rating = reviewDto.Rating;
            review.Comment = reviewDto.Comment;

            await _reviewRepository.UpdateAsync(review);
        }

        public async Task DeleteReviewAsync(int id)
        {
            await _reviewRepository.DeleteAsync(id);
        }
    }
}
