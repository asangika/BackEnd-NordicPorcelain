using Ecommerce.Domain.Enums;
using Ecommerce.Domain.src.Entities.OrderAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Presentation.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : AppController<Order, OrderReadDto, OrderCreateDto, OrderUpdateDto>
    {
        private readonly IOrderManagement _orderManagement;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderManagement orderManagement, ILogger<OrderController> logger) : base(orderManagement)
        {
            _orderManagement = orderManagement;
            _logger = logger;

        }

        // GET: api/v1/order/{userId}
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetOrdersByUserId(Guid userId)
        {
            _logger.LogInformation("Received request to get orders for user ID: {UserId}", userId);

            var orders = await _orderManagement.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        // GET: api/v1/order/status/{status}
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderManagement.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        // GET: api/v1/order/date-range
        [HttpGet("date-range")]
        public async Task<IActionResult> GetOrdersByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var orders = await _orderManagement.GetOrdersByDateRangeAsync(startDate, endDate);
            return Ok(orders);
        }

        // GET: api/v1/order/total-price/{orderId}
        [HttpGet("total-price/{orderId}")]
        public async Task<IActionResult> GetTotalPriceByOrderId(Guid orderId)
        {
            try
            {
                var totalPrice = await _orderManagement.GetTotalPriceByOrderIdAsync(orderId);
                return Ok(new { OrderId = orderId, TotalPrice = totalPrice });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        public override async Task<ActionResult<OrderReadDto>> CreateAsync(OrderCreateDto entity)
        {
            var result = await base.CreateAsync(entity);

            if (result.Result is CreatedAtActionResult createdAtActionResult)
            {
                var createdOrder = (OrderReadDto)createdAtActionResult.Value;
                return CreatedAtAction(nameof(CreateAsync), new { id = createdOrder.OrderId }, createdOrder);
            }
            return StatusCode(500, "Error creating the order.");
        }

    }
}
