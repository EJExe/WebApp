// Controllers/CarsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.Services;
using ProjectManagement.Domain.Entities;
using System.Collections.Generic;

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

        // GET: api/cars
        [HttpGet]
        public ActionResult<IEnumerable<Car>> GetAllCars()
        {
            var cars = _carService.GetAllCars();
            return Ok(cars);
        }

        // GET: api/cars/5
        [HttpGet("{id}")]
        public ActionResult<Car> GetCarById(int id)
        {
            var car = _carService.GetCarById(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        // POST: api/cars
        [HttpPost]
        public ActionResult<Car> AddCar([FromBody] Car car)
        {
            _carService.AddCar(car);
            return CreatedAtAction(nameof(GetCarById), new { id = car.Id }, car);
        }

        // PUT: api/cars/5
        [HttpPut("{id}")]
        public IActionResult UpdateCar(int id, [FromBody] Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            _carService.UpdateCar(car);
            return NoContent();
        }

        // DELETE: api/cars/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCar(int id)
        {
            _carService.DeleteCar(id);
            return NoContent();
        }
    }
}