// RentalApplicationsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;
using System.Security.Claims;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/rental-applications")]
    [ApiController]
    [Authorize]
    public class RentalApplicationsController : ControllerBase
    {
        private readonly RentalApplicationService _service;
        private readonly ILogger<RentalApplicationsController> _logger;

        public RentalApplicationsController(
            RentalApplicationService service,
            ILogger<RentalApplicationsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<RentalApplicationDto>> Create([FromBody] CreateRentalApplicationDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var application = await _service.CreateApplicationAsync(dto, userId);
                return CreatedAtAction(nameof(GetById), new { id = application.Id }, application);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating rental application");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentalApplicationDto>> GetById(int id)
        {
            try
            {
                var application = await _service.GetApplicationByIdAsync(id);
                return Ok(application);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting rental application with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{id}/approve")]
        public async Task<ActionResult<RentalApplicationDto>> Approve(int id)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var application = await _service.ApproveApplicationAsync(id, adminId);
                return Ok(application);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error approving rental application with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{id}/reject")]
        public async Task<ActionResult<RentalApplicationDto>> Reject(int id)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var application = await _service.RejectApplicationAsync(id, adminId);
                return Ok(application);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error rejecting rental application with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalApplicationDto>>> GetAll()
        {
            try
            {
                var applications = await _service.GetAllApplicationsAsync();
                return Ok(applications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all rental applications");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}