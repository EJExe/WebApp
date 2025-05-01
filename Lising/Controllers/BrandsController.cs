using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;

namespace Lising.Controllers
{
    [Route("api/brands")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly BrandService _service;
        private readonly ILogger<BrandsController> _logger;

        public BrandsController(
            BrandService service,
            ILogger<BrandsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAll()
        {
            try
            {
                var brands = await _service.GetAllBrandsAsync();
                return Ok(brands);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении брендов");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetById(int id)
        {
            try
            {
                var brand = await _service.GetBrandByIdAsync(id);
                return brand == null ? NotFound() : Ok(brand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении бренда ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        
        [HttpPost]
        public async Task<ActionResult<BrandDto>> Create([FromBody] CreateBrandDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.CreateBrandAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBrandDto dto)
        {
            if (id != dto.Id) return BadRequest("Несоответствие ID");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _service.UpdateBrandAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении бренда ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteBrandAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении бренда ID: {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
