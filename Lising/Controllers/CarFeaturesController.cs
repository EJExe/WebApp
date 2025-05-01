using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;

namespace Lising.Controllers
{
    [Route("api/CarFeatures")]
    [ApiController]
    public class CarFeaturesController : ControllerBase
    {
        private readonly CarFeatureService _featureService;
        private readonly ILogger<CarFeaturesController> _logger;

        public CarFeaturesController(CarFeatureService featureService, ILogger<CarFeaturesController> logger)
        {
            _featureService = featureService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarFeatureDto>>> GetAll()
        {
            try
            {
                var features = await _featureService.GetAllFeaturesAsync();
                return Ok(features);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting car features");
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarFeatureDto>> GetById(int id)
        {
            try
            {
                var feature = await _featureService.GetFeatureByIdAsync(id);
                if (feature == null) return NotFound();
                return Ok(feature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting feature with id {id}");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<CarFeatureDto>> Create([FromBody] CreateCarFeatureDto featureDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var createdFeature = await _featureService.CreateFeatureAsync(featureDto);
                return CreatedAtAction(nameof(GetById), new { id = createdFeature.Id }, createdFeature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating car feature");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCarFeatureDto featureDto)
        {
            if (id != featureDto.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _featureService.UpdateFeatureAsync(featureDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating feature with id {id}");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _featureService.DeleteFeatureAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting feature with id {id}");
                return StatusCode(500);
            }
        }
    }
}
