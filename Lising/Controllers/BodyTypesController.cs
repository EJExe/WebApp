using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Api.Controllers;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;
using System.Data;
using System.Security.Claims;

namespace Lising.Controllers
{
    [Route("api/BodyTypes")]
    [ApiController]
    public class BodyTypesController : ControllerBase
    {
        private readonly BodyTypeService _service;
        private readonly ILogger<CarsController> _logger;

        public BodyTypesController(
            BodyTypeService service,
            ILogger<CarsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BodyTypeDto>>> GetAll()
        {
            try
            {
                var bodyTypes = await _service.GetAllBodyTypesAsync();
                return Ok(bodyTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении типов кузова");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BodyTypeDto>> GetById(int id)
        {
            try
            {
                var bodyType = await _service.GetBodyTypeByIdAsync(id);
                return bodyType == null ? NotFound() : Ok(bodyType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении типа кузова ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BodyTypeDto>> Create([FromBody] CreateBodyTypeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _logger.LogInformation("User roles: {Roles}", User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value));
            await _service.CreateBodyTypeAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id  }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBodyTypeDto dto)
        {
            if (id != dto.Id) return BadRequest("Несоответствие ID");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _service.UpdateBodyTypeAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении типа кузова ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteBodyTypeAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении типа кузова ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
