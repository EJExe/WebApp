using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Interfaces
{
    public interface ICarFeatureRepository
    {
        Task<IEnumerable<CarFeature>> GetAllAsync();
        Task<CarFeature> GetByIdAsync(int id);
        Task AddAsync(CarFeature feature);
        Task UpdateAsync(CarFeature feature);
        Task DeleteAsync(int id);
    }
}
