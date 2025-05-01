using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;

//namespace ProjectManagement.Application.Services
//{
//    public class CarCategoryService
//    {
//        private readonly ICarCategoryRepository _carCategoryRepository;

//        public CarCategoryService(ICarCategoryRepository carCategoryRepository)
//        {
//            _carCategoryRepository = carCategoryRepository;
//        }

//        public async Task<IEnumerable<CarCategoryDto>> GetAllCategoriesAsync()
//        {
//            var categories = await _carCategoryRepository.GetAllAsync();
//            return categories.Select(c => new CarCategoryDto
//            {
//                Id = c.Id,
//                Name = c.Name,
//                Description = c.Description
//            });
//        }

//        public async Task<CarCategoryDto> GetCategoryByIdAsync(int id)
//        {
//            var category = await _carCategoryRepository.GetByIdAsync(id);
//            return category == null ? null : new CarCategoryDto
//            {
//                Id = category.Id,
//                Name = category.Name,
//                Description = category.Description
//            };
//        }

//        public async Task CreateCategoryAsync(CarCategoryDto categoryDto)
//        {
//            var category = new CarCategory
//            {
//                Name = categoryDto.Name,
//                Description = categoryDto.Description
//            };
//            await _carCategoryRepository.AddAsync(category);
//        }

//        public async Task UpdateCategoryAsync(CarCategoryDto categoryDto)
//        {
//            var category = await _carCategoryRepository.GetByIdAsync(categoryDto.Id);
//            if (category != null)
//            {
//                category.Name = categoryDto.Name;
//                category.Description = categoryDto.Description;
//                await _carCategoryRepository.UpdateAsync(category);
//            }
//        }

//        public async Task DeleteCategoryAsync(int id)
//        {
//            await _carCategoryRepository.DeleteAsync(id);
//        }
//    }
namespace ProjectManagement.Application.Services
{
    public class CarCategoryService
    {
        private readonly ICarCategoryRepository _repository;

        public CarCategoryService(ICarCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CarCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(c => new CarCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public async Task<CarCategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category == null ? null : new CarCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task CreateCategoryAsync(CreateCarCategoryDto dto)
        {
            var category = new CarCategory
            {
                Name = dto.Name,
                Description = dto.Description
            };
            await _repository.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(UpdateCarCategoryDto dto)
        {
            var category = await _repository.GetByIdAsync(dto.Id);
            if (category != null)
            {
                category.Name = dto.Name;
                category.Description = dto.Description;
                await _repository.UpdateAsync(category);
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
