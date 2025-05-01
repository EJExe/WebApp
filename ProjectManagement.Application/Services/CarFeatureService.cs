using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Domain.Interfaces;

namespace ProjectManagement.Application.Services
{
    public class CarFeatureService
    {
        private readonly ICarFeatureRepository _carFeatureRepository;

        public CarFeatureService(ICarFeatureRepository carFeatureRepository)
        {
            _carFeatureRepository = carFeatureRepository;
        }

        public async Task<IEnumerable<CarFeatureDto>> GetAllFeaturesAsync()
        {
            var features = await _carFeatureRepository.GetAllAsync();
            return features.Select(f => new CarFeatureDto
            {
                Id = f.Id,
                Name = f.Name
            });
        }

        public async Task<CarFeatureDto> GetFeatureByIdAsync(int id)
        {
            var feature = await _carFeatureRepository.GetByIdAsync(id);
            return feature == null
                ? null
                : new CarFeatureDto
                {
                    Id = feature.Id,
                    Name = feature.Name
                };
        }

        public async Task<CarFeatureDto> CreateFeatureAsync(CreateCarFeatureDto featureDto)
        {
            var feature = new CarFeature
            {
                Name = featureDto.Name
            };
            await _carFeatureRepository.AddAsync(feature);
            return new CarFeatureDto { Id = feature.Id, Name = feature.Name };
        }

        public async Task UpdateFeatureAsync(UpdateCarFeatureDto featureDto)
        {
            var feature = await _carFeatureRepository.GetByIdAsync(featureDto.Id);
            if (feature != null)
            {
                feature.Name = featureDto.Name;
                await _carFeatureRepository.UpdateAsync(feature);
            }
            else
            {
                throw new KeyNotFoundException("Feature not found");
            }
        }

        public async Task DeleteFeatureAsync(int id)
        {
            await _carFeatureRepository.DeleteAsync(id);
        }
    }
}
