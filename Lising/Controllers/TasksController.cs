using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lising.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks([FromQuery] string? title)
        {
            // Если параметр title не указан, возвращаем все задачи
            var tasks = string.IsNullOrEmpty(title)
                ? await _taskService.GetTasksAsync()
                : await _taskService.SearchTasksByTitleAsync(title);

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask(TaskDto AppTaskDto)
        {
            await _taskService.AddTaskAsync(AppTaskDto);
            return CreatedAtAction(nameof(GetTask), new { id = AppTaskDto.Id }, AppTaskDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskDto AppTaskDto)
        {
            if (id != AppTaskDto.Id) return BadRequest();
            await _taskService.UpdateTaskAsync(AppTaskDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}
