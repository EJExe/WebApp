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
    public class BodyTypeService
    {
        private readonly IBodyTypeRepository _repository;
        private readonly ILogger<BodyTypeService> _logger;

        public BodyTypeService(
            IBodyTypeRepository repository,
            ILogger<BodyTypeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<BodyTypeDto>> GetAllBodyTypesAsync()
        {
            try
            {
                var bodyTypes = await _repository.GetAllAsync();
                return bodyTypes.Select(bt => new BodyTypeDto
                {
                    Id = bt.Id,
                    Name = bt.Name
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении типов кузова");
                throw;
            }
        }

        public async Task<BodyTypeDto> GetBodyTypeByIdAsync(int id)
        {
            try
            {
                var bodyType = await _repository.GetByIdAsync(id);
                return bodyType == null
                    ? null
                    : new BodyTypeDto { Id = bodyType.Id, Name = bodyType.Name };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении типа кузова ID: {id}");
                throw;
            }
        }

        public async Task<BodyTypeDto> CreateBodyTypeAsync(CreateBodyTypeDto dto)
        {
            try
            {
                var bodyType = new BodyType { Name = dto.Name };
                await _repository.AddAsync(bodyType);
                return new BodyTypeDto { Id = bodyType.Id, Name = bodyType.Name };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании типа кузова");
                throw;
            }
        }

        public async Task UpdateBodyTypeAsync(UpdateBodyTypeDto dto)
        {
            try
            {
                var bodyType = await _repository.GetByIdAsync(dto.Id);
                if (bodyType == null)
                    throw new KeyNotFoundException("Тип кузова не найден");

                bodyType.Name = dto.Name;
                await _repository.UpdateAsync(bodyType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении типа кузова ID: {dto.Id}");
                throw;
            }
        }

        public async Task DeleteBodyTypeAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении типа кузова ID: {id}");
                throw;
            }
        }
    }
} 
