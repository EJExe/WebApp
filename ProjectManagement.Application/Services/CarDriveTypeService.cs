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
    public class CarDriveTypeService
    {
        private readonly ICarDriveTypeRepository _repository;

        public CarDriveTypeService(ICarDriveTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DriveTypeDto>> GetAllDriveTypesAsync()
        {
            var driveTypes = await _repository.GetAllAsync();
            return driveTypes.Select(dt => new DriveTypeDto { Id = dt.Id, Name = dt.Name });
        }

        public async Task<DriveTypeDto> GetDriveTypeByIdAsync(int id)
        {
            var driveType = await _repository.GetByIdAsync(id);
            return driveType == null
                ? null
                : new DriveTypeDto { Id = driveType.Id, Name = driveType.Name };
        }

        public async Task CreateDriveTypeAsync(CreateCarDriveTypeDto dto)
        {
            var driveType = new CarDriveType { Name = dto.Name };
            await _repository.AddAsync(driveType);
        }

        public async Task UpdateDriveTypeAsync(UpdateCarDriveTypeDto dto)
        {
            var driveType = await _repository.GetByIdAsync(dto.Id);
            if (driveType != null)
            {
                driveType.Name = dto.Name;
                await _repository.UpdateAsync(driveType);
            }
        }

        public async Task DeleteDriveTypeAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
