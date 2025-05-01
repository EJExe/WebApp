using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelTypesController : ControllerBase
    {
        private readonly FuelTypeService _service;
        private readonly ILogger<FuelTypesController> _logger;

        public FuelTypesController(
            FuelTypeService service,
            ILogger<FuelTypesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuelTypeDto>>> GetAll()
        {
            try
            {
                var fuelTypes = await _service.GetAllFuelTypesAsync();
                return Ok(fuelTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении типов топлива");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FuelTypeDto>> GetById(int id)
        {
            try
            {
                var fuelType = await _service.GetFuelTypeByIdAsync(id);
                return fuelType == null ? NotFound() : Ok(fuelType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении типа топлива ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<FuelTypeDto>> Create([FromBody] CreateFuelTypeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _service.CreateFuelTypeAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании типа топлива");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFuelTypeDto dto)
        {
            if (id != dto.Id) return BadRequest("Несоответствие ID");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _service.UpdateFuelTypeAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении типа топлива ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteFuelTypeAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении типа топлива ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}