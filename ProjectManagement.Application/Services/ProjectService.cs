using ProjectManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProjectManagement.Application.Services
{
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;

        public ProjectService(IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsAsync()
        {
            var projects = await _projectRepository.GetProjectsAsync();
            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                DueDate = p.DueDate
            });
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null) return null;
            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                DueDate = project.DueDate,
                Tasks = project.Tasks.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    ProjectId = t.ProjectId,
                    UserId = t.UserId
                }).ToList() // Конвертируем связанные задачи в TaskDto
            };
        }

        public async Task<List<TaskDto>> GetTasksForProjectAsync(int projectId)
        {
            var tasks = await _projectRepository.GetTasksByProjectIdAsync(projectId);

            if (tasks == null || tasks.Count == 0)
                return null;

            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                ProjectId = t.ProjectId,
                UserId = t.UserId
            }).ToList();
        }

        public async Task AddProjectAsync(CreateProjectDto projectDto)
        {
            var project = new Project
            {
                Title = projectDto.Title,
                Description = projectDto.Description,
                DueDate = projectDto.DueDate,
            };
            await _projectRepository.AddProjectAsync(project);
            projectDto.Id = project.Id;
        }

        public async Task UpdateProjectAsync(CreateProjectDto projectDto)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectDto.Id);
            if (project != null)
            {
                project.Title = projectDto.Title;
                project.Description = projectDto.Description;
                project.DueDate = projectDto.DueDate;
                await _projectRepository.UpdateProjectAsync(project);
            }
        }

        public async Task DeleteProjectAsync(int id)
        {
            try
            {
                await _projectRepository.DeleteProjectAsync(id);
            }
            catch (InvalidOperationException ex)
            {
                // Обработка исключения, если проект не может быть удален
                throw new ApplicationException($"Error deleting project: {ex.Message}", ex);
            }
        }
    }
}
