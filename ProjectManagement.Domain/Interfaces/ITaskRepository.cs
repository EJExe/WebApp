using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<AppTask> GetTaskByIdAsync(int id);
        Task<IEnumerable<AppTask>> GetTasksAsync();
        Task<IEnumerable<AppTask>> SearchTasksByTitleAsync(string title);
        Task AddTaskAsync(AppTask task);
        Task UpdateTaskAsync(AppTask task);
        Task DeleteTaskAsync(int id);
    }
}
