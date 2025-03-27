using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Lising.Controllers
{
    namespace ProjectManagement.Api.Controllers
    {
        [ApiController]
        // Атрибут, указывающий, что данный класс является контроллером API.

        [Route("api/[controller]")]
        // Определяет маршрут для запросов API (в данном случае "api/Projects").

        public class ProjectsController : ControllerBase
        {
            private readonly ProjectService _projectService;

            public ProjectsController(ProjectService projectService)
            // Конструктор, инициализирующий зависимость ProjectService через внедрение зависимостей (DI).
            {
                _projectService = projectService;
            }

            [HttpGet]
            // Указывает, что данный метод обрабатывает HTTP GET-запросы.
            public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
            // Метод для получения списка всех проектов.
            {
                var projects = await _projectService.GetProjectsAsync();
                // Асинхронное обращение к сервису для получения списка проектов.
                return Ok(projects);
                // Возвращает HTTP-ответ 200 (OK) с данными проектов.
            }

            [HttpGet("{id}")]
            // Указывает, что данный метод обрабатывает HTTP GET-запросы по ID.
            public async Task<ActionResult<ProjectDto>> GetProject(int id)
            {
                var project = await _projectService.GetProjectByIdAsync(id);
                if (project == null) return NotFound();
                // Если проект не найден, возвращается HTTP-ответ 404 (Not Found).
                return Ok(project);
            }

            [HttpGet("{id}/tasks")]
            // Указывает, что данный метод обрабатывает HTTP GET-запросы по маршруту "/api/projects/{id}/tasks".
            public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksForProject(int id)
            {
                var tasks = await _projectService.GetTasksForProjectAsync(id);
                if (tasks == null || !tasks.Any())
                    return NotFound($"No tasks found for project with ID {id}.");
                // Возвращает HTTP-ответ 404 (Not Found), если задач нет.
                return Ok(tasks);
                // Возвращает HTTP-ответ 200 (OK) с данными задач.
            }

            [HttpPost]
            // Указывает, что данный метод обрабатывает HTTP POST-запросы.
            public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto projectDto)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                await _projectService.AddProjectAsync(projectDto);
                return CreatedAtAction(nameof(GetProject), new { id = projectDto.Id }, projectDto);
                // Возвращает HTTP-ответ 201 (Created) с URL созданного проекта.
            }

            [HttpPut("{id}")]
            // Указывает, что данный метод обрабатывает HTTP PUT-запросы для обновления.
            public async Task<ActionResult<ProjectDto>> UpdateProject(int id, CreateProjectDto projectDto)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (id != projectDto.Id) return BadRequest();
                await _projectService.UpdateProjectAsync(projectDto);
                return CreatedAtAction(nameof(GetProject), new { id = projectDto.Id }, projectDto);
            }

            //[Authorize(Roles = "admin")]
            [HttpDelete("{id}")]
            // Указывает, что данный метод обрабатывает HTTP DELETE-запросы.
            public async Task<IActionResult> DeleteProject(int id)
            {
                try
                {
                    await _projectService.DeleteProjectAsync(id);
                    return NoContent();
                    // Возвращает HTTP-ответ 204 (No Content) после успешного удаления.
                }
                catch (ApplicationException ex)
                {
                    return Conflict(new { message = ex.Message });
                    // В случае ошибки возвращает HTTP-ответ 409 (Conflict) с описанием ошибки.
                }
            }
        }
    }
}
