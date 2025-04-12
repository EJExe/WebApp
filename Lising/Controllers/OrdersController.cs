using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        // Внедрение зависимости через конструктор
        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var orderDto = await _orderService.GetOrderByIdAsync(id);
            if (orderDto == null)
            {
                return NotFound(); // Теперь NotFound доступен
            }
            return Ok(orderDto); // Теперь Ok доступен
        }
    }
}