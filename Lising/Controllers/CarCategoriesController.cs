using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;
using ProjectManagement.Domain.Interfaces;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/CarCategories")]
    [ApiController]
    public class CarCategoriesController : ControllerBase
    {
        private readonly CarCategoryService _service;
        private readonly ILogger<CarCategoriesController> _logger;

        public CarCategoriesController(
            CarCategoryService service,
            ILogger<CarCategoriesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarCategoryDto>>> GetAll()
        {
            try
            {
                var categories = await _service.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении категорий автомобилей");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarCategoryDto>> GetById(int id)
        {
            try
            {
                var category = await _service.GetCategoryByIdAsync(id);
                return category == null ? NotFound() : Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении категории ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<CarCategoryDto>> Create([FromBody] CreateCarCategoryDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _service.CreateCategoryAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании категории");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCarCategoryDto dto)
        {
            if (id != dto.Id) return BadRequest("Несоответствие ID");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _service.UpdateCategoryAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении категории ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении категории ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}