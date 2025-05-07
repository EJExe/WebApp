using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;
using ProjectManagement.Domain.Entities;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/Cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly CarService _carService;
        private readonly ILogger<CarsController> _logger;

        public CarsController(CarService carService, ILogger<CarsController> logger)
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
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<CarSearchResultDto>> SearchCars([FromQuery] CarFilterDto filter)
        {
            try
            {
                var result = await _carService.SearchCarsAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching cars");
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetCar(int id)
        {
            try
            {
                var car = await _carService.GetCarByIdAsync(id);
                if (car == null) return NotFound();
                if (!string.IsNullOrEmpty(car.ImagePath))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), car.ImagePath.TrimStart('/'));
                    if (!System.IO.File.Exists(filePath))
                    {
                        car.ImagePath = "/images/cars/default.jpg"; // Указываем путь по умолчанию
                    }
                }
                else
                {
                    car.ImagePath = "/images/cars/default.jpg";
                }
                return Ok(car);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting car with ID {Id}", id);
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CarDto>> CreateCar(
            [FromForm] CreateCarDto dto,
            [FromForm] IFormFile? Image)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for CreateCar: {Errors}", string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            // Validate image if provided
            if (Image != null)
            {
                var validTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
                if (!validTypes.Contains(Image.ContentType))
                    return BadRequest("Invalid image type. Only JPEG and PNG are allowed.");
                if (Image.Length > 5 * 1024 * 1024) // 5MB limit
                    return BadRequest("Image size exceeds 5MB.");

                var fileName = $"{Guid.NewGuid()}_{Image.FileName}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", "cars", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // Создаем папку, если не существует

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                dto.ImagePath = $"/images/cars/{fileName}";
            }
            else if (!string.IsNullOrEmpty(dto.ImagePath))
            {
                // Проверка, существует ли указанный файл
                var normalizedPath = dto.ImagePath.Replace('\\', '/').TrimStart('/');
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), normalizedPath);
                if (System.IO.File.Exists(filePath))
                {
                    dto.ImagePath = $"/{normalizedPath}";
                }
                else
                {
                    _logger.LogWarning($"Image file not found: {filePath}. Using default image.");
                    dto.ImagePath = "/images/cars/default.jpg";
                }
            }
            else
            {
                // Set default image path if none provided
                dto.ImagePath = "/images/cars/default.jpg";
                _logger.LogInformation("No image provided, using default image path: {ImagePath}", dto.ImagePath);
            }

            try
            {
                var createdCar = await _carService.AddCarAsync(dto, Image);
                _logger.LogInformation("Car created successfully with ID: {Id}", createdCar.Id);
                return CreatedAtAction(nameof(GetCar), new { id = createdCar.Id }, createdCar);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid input data for car creation");
                return BadRequest(new { Message = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating car");
                return StatusCode(500, new { Message = "Database error", Details = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating car");
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromForm] UpdateCarDto dto, [FromForm] IFormFile Image)
        {
            try
            {
                _logger.LogInformation($"UpdateCar request: id={id}, dto={System.Text.Json.JsonSerializer.Serialize(dto)}, Image={(Image != null ? Image.FileName : "null")}");

                if (id != dto.Id)
                {
                    _logger.LogWarning($"ID mismatch: URL id={id}, DTO id={dto.Id}");
                    return BadRequest(new { Message = "ID mismatch between URL and body" });
                }

                // Проверка обязательных полей
                if (dto.BrandId <= 0) return BadRequest(new { Message = "Invalid BrandId" });
                if (string.IsNullOrEmpty(dto.Model)) return BadRequest(new { Message = "Model is required" });
                if (dto.Year < 1900) return BadRequest(new { Message = "Invalid Year" });
                if (dto.BodyTypeId <= 0) return BadRequest(new { Message = "Invalid BodyTypeId" });
                if (dto.CategoryId <= 0) return BadRequest(new { Message = "Invalid CategoryId" });
                if (dto.DriveTypeId <= 0) return BadRequest(new { Message = "Invalid DriveTypeId" });
                if (dto.FuelTypeId <= 0) return BadRequest(new { Message = "Invalid FuelTypeId" });

                // Логирование входных данных
                _logger.LogInformation($"Received UpdateCar request: id={id}, dto.Id={dto.Id}, IsLeasingDisabled={dto.IsLeasingDisabled}, ImagePath={dto.ImagePath}, Image={(Image != null ? Image.FileName : "null")}");

                // Обработка изображения
                if (Image != null)
                {
                    var validTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
                    if (!validTypes.Contains(Image.ContentType)) { 
                        _logger.LogWarning($"Invalid image type: {Image.ContentType}");
                        return BadRequest(new { Message = "Invalid image type. Only JPEG and PNG are allowed." });
                    }
                    if (Image.Length > 5 * 1024 * 1024)
                    {
                        _logger.LogWarning($"Image size exceeds 5MB: {Image.Length} bytes");
                        return BadRequest(new { Message = "Image size exceeds 5MB." });
                    }

                    var fileName = $"{Guid.NewGuid()}_{Image.FileName}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", "cars", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    dto.ImagePath = $"/images/cars/{fileName}";
                }
                else if (string.IsNullOrEmpty(dto.ImagePath))
                {
                    dto.ImagePath = "/images/cars/default.jpg";
                }

                await _carService.UpdateCarAsync(id, dto, Image);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Car not found");
                return NotFound(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument");
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during car update");
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred",
                    Details = ex.Message,
                    InnerException = ex.InnerException?.Message
                });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                await _carService.DeleteCarAsync(id);
                _logger.LogInformation("Car with ID {Id} deleted successfully", id);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting car with ID {Id}", id);
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting car");
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }
    }
}