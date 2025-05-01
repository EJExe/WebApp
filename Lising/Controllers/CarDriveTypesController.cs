using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/DriveType")]
    [ApiController]
    public class CarDriveTypesController : ControllerBase
    {
        private readonly CarDriveTypeService _service;
        private readonly ILogger<CarDriveTypesController> _logger;

        public CarDriveTypesController(
            CarDriveTypeService service,
            ILogger<CarDriveTypesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriveTypeDto>>> GetAll()
        {
            try
            {
                var driveTypes = await _service.GetAllDriveTypesAsync();
                return Ok(driveTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении типов привода");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DriveTypeDto>> GetById(int id)
        {
            try
            {
                var driveType = await _service.GetDriveTypeByIdAsync(id);
                return driveType == null ? NotFound() : Ok(driveType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении типа привода ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<DriveTypeDto>> Create([FromBody] CreateCarDriveTypeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _service.CreateDriveTypeAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании типа привода");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCarDriveTypeDto dto)
        {
            if (id != dto.Id) return BadRequest("Несоответствие ID");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _service.UpdateDriveTypeAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении типа привода ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteDriveTypeAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении типа привода ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}