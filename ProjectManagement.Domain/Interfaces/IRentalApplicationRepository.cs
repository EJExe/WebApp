// ProjectManagement.Domain\Interfaces\IRentalApplicationRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Domain.Interfaces
{
    public interface IRentalApplicationRepository
    {
        Task<IEnumerable<RentalApplication>> GetAllAsync();
        Task<RentalApplication> GetByIdAsync(int id);
        Task AddAsync(RentalApplication application);
        Task UpdateAsync(RentalApplication application);
        Task DeleteAsync(int id);
    }
}