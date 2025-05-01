using Lising.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/Cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly CarService _carService;
        private readonly ILogger<BodyTypesController> _logger;

        public CarsController(CarService carService,
            ILogger<BodyTypesController> logger)
        {
            _carService = carService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<CarSearchResultDto>> GetCars(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _carService.GetCarsWithPaginationAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cars");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetCar(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        
        [HttpPost]
        public async Task<ActionResult<CarDto>> CreateCar(CreateCarDto CreatedcarDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _carService.AddCarAsync(CreatedcarDto);
            return CreatedAtAction(nameof(GetCar), new { id = CreatedcarDto.Id }, CreatedcarDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, UpdateCarDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { Message = "ID mismatch between URL and body" });

            try
            {
                await _carService.UpdateCarAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Car update error");
                return StatusCode(500, new
                {
                    Message = "Operation failed",
                    Details = ex.InnerException?.Message
                });
            }
        }

        //public async Task<IActionResult> UpdateCar(int id, UpdateCarDto dto)
        //{
        //    if (id != dto.Id) return BadRequest();
        //    await _carService.UpdateCarAsync(id, dto);
        //    return CreatedAtAction(nameof(GetCar), new { id = dto.Id }, dto);
        //}


        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                await _carService.DeleteCarAsync(id);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}