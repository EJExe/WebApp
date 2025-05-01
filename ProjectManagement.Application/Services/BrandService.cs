using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;

namespace ProjectManagement.Application.Services
{
    public class BrandService
    {
        private readonly IBrandRepository _repository;
        private readonly ILogger<BrandService> _logger;

        public BrandService(IBrandRepository repository, ILogger<BrandService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            try
            {
                var brands = await _repository.GetAllAsync();
                return brands.Select(b => new BrandDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Country = b.Country,
                    LogoUrl = b.LogoUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении брендов");
                throw;
            }
        }

        public async Task<BrandDto> GetBrandByIdAsync(int id)
        {
            try
            {
                var brand = await _repository.GetByIdAsync(id);
                return brand == null
                    ? null
                    : new BrandDto
                    {
                        Id = brand.Id,
                        Name = brand.Name,
                        Country = brand.Country,
                        LogoUrl = brand.LogoUrl
                    };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении бренда ID: {id}");
                throw;
            }
        }

        public async Task<BrandDto> CreateBrandAsync(CreateBrandDto dto)
        {
            try
            {
                var brand = new Brand
                {
                    Name = dto.Name,
                    Country = dto.Country,
                    LogoUrl = dto.LogoUrl
                };

                await _repository.AddAsync(brand);
                return new BrandDto
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    Country = brand.Country,
                    LogoUrl = brand.LogoUrl
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании бренда");
                throw;
            }
        }

        public async Task UpdateBrandAsync(UpdateBrandDto dto)
        {
            try
            {
                var brand = await _repository.GetByIdAsync(dto.Id);
                if (brand == null)
                    throw new KeyNotFoundException("Бренд не найден");

                brand.Name = dto.Name;
                brand.Country = dto.Country;
                brand.LogoUrl = dto.LogoUrl;

                await _repository.UpdateAsync(brand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении бренда ID: {dto.Id}");
                throw;
            }
        }

        public async Task DeleteBrandAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении бренда ID: {id}");
                throw;
            }
        }
    }
}
