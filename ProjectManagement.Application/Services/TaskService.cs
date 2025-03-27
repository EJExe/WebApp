using ProjectManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksAsync()
        {
            var tasks = await _taskRepository.GetTasksAsync();
            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                ProjectId = t.ProjectId,
                UserId = t.UserId
            });
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return null;
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                ProjectId = task.ProjectId,
                UserId = task.UserId
            };
        }


        public async Task<IEnumerable<TaskDto>> SearchTasksByTitleAsync(string title)
        {
            var tasks = await _taskRepository.SearchTasksByTitleAsync(title);
            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                ProjectId = t.ProjectId,
                UserId = t.UserId
            });
        }

        public async Task AddTaskAsync(TaskDto AppTaskDto)
        {
            var task = new AppTask
            {
                Title = AppTaskDto.Title,
                Description = AppTaskDto.Description,
                ProjectId = AppTaskDto.ProjectId,
                UserId = AppTaskDto.UserId
            };
            await _taskRepository.AddTaskAsync(task);
        }

        public async Task UpdateTaskAsync(TaskDto AppTaskDto)
        {
            var task = await _taskRepository.GetTaskByIdAsync(AppTaskDto.Id);
            if (task != null)
            {
                task.Title = AppTaskDto.Title;
                task.Description = AppTaskDto.Description;
                task.ProjectId = AppTaskDto.ProjectId;
                task.UserId = AppTaskDto.UserId;
                await _taskRepository.UpdateTaskAsync(task);
            }
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteTaskAsync(id);
        }
    }
}
