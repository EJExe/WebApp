using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Services;
using System.Security.Claims;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly ILogger<OrdersController> _logger;
        private readonly AppDbContext _context;

        public OrdersController(OrderService orderService, ILogger<OrdersController> logger, AppDbContext context)
        {
            _orderService = orderService;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDto orderDto)
        {
            await _orderService.AddOrderAsync(orderDto);
            return CreatedAtAction(nameof(Get), new { id = orderDto.OrderId }, orderDto);
        }

        
        [HttpPost("{orderId}/complete")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            
            var result = await _orderService.CompleteOrder(orderId);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        
        [HttpPut("{orderId}/confirm")]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            var result = await _orderService.ConfirmOrder(orderId);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        
        [HttpPut("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var result = await _orderService.CancelOrder(orderId);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderDto orderDto)
        {
            if (id != orderDto.OrderId)
                return BadRequest();

            await _orderService.UpdateOrderAsync(orderDto);
            return NoContent();
        }
    }
} 