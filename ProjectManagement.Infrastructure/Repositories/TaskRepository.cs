using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppTask> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<AppTask>> GetTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppTask>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppTask>> GetTasksByUserIdAsync(string userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppTask>> SearchTasksByTitleAsync(string title)
        {
            return await _context.Tasks
                .Where(t => t.Title.Contains(title))
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task AddTaskAsync(AppTask task)
        {
            // Проверяем, существует ли проект с указанным ProjectId
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == task.ProjectId);

            if (!projectExists)
            {
                throw new ArgumentException($"Project with ID {task.ProjectId} does not exist.");
            }

            // Если проект существует, добавляем задачу
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(AppTask task)
        {
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == task.ProjectId);

            if (!projectExists)
            {
                throw new ArgumentException($"Project with ID {task.ProjectId} does not exist.");
            }

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
