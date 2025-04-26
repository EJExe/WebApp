using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly CarService _carService;

        public CarsController(CarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetCars()
        {
            var cars = await _carService.GetCarsAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetCar(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        
        [HttpPost]
        public async Task<ActionResult<CarDto>> CreateCar(CreateCarDto carDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _carService.AddCarAsync(carDto);
            return CreatedAtAction(nameof(GetCar), new { id = carDto.Id }, carDto);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, CreateCarDto carDto)
        {
            if (id != carDto.Id) return BadRequest();
            await _carService.UpdateCarAsync(carDto);
            return CreatedAtAction(nameof(GetCar), new { id = carDto.Id }, carDto);
        }


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