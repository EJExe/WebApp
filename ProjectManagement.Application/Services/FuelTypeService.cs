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
    public class FuelTypeService
    {
        private readonly IFuelTypeRepository _repository;

        public FuelTypeService(IFuelTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FuelTypeDto>> GetAllFuelTypesAsync()
        {
            var fuelTypes = await _repository.GetAllAsync();
            return fuelTypes.Select(ft => new FuelTypeDto { Id = ft.Id, Name = ft.Name });
        }

        public async Task<FuelTypeDto> GetFuelTypeByIdAsync(int id)
        {
            var fuelType = await _repository.GetByIdAsync(id);
            return fuelType == null
                ? null
                : new FuelTypeDto { Id = fuelType.Id, Name = fuelType.Name };
        }

        public async Task CreateFuelTypeAsync(CreateFuelTypeDto dto)
        {
            var fuelType = new FuelType { Name = dto.Name };
            await _repository.AddAsync(fuelType);
        }

        public async Task UpdateFuelTypeAsync(UpdateFuelTypeDto dto)
        {
            var fuelType = await _repository.GetByIdAsync(dto.Id);
            if (fuelType != null)
            {
                fuelType.Name = dto.Name;
                await _repository.UpdateAsync(fuelType);
            }
        }

        public async Task DeleteFuelTypeAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
