using System.Threading.Tasks;
using Agora.Application.DTOs;
using Agora.Application.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Agora.Application.Common;

namespace Agora.API.Controller
{
    [Route("api/[controller]")]
    [Authorize] 
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }
            return int.Parse(userIdClaim.Value);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            try
            {
                int userId = GetUserId();
                request.UserId = userId;
                _logger.LogInformation("\u001b[32m[Order]\u001b[0m {UserId} is checking out with request: {@Request}", userId, request);
                var result = await _orderService.CheckoutAsync(request);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "\u001b[31m[Order]\u001b[0m Error during checkout for user: {Message}",  ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] PagedRequest pagedRequest)
        {
            try
            {
                int userId = GetUserId();
                _logger.LogInformation("\u001b[32m[Order]\u001b[0m {UserId} is retrieving orders with pagination: {@PagedRequest}", userId, pagedRequest);
                var result = await _orderService.GetOrders(userId, pagedRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\u001b[31m[Order]\u001b[0m Error retrieving orders for user: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("controller/{userId}")]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> GetOrdersByUserId(int userId, [FromQuery] PagedRequest pagedRequest)
        {
            try
            {
                _logger.LogInformation("\u001b[32m[Order]\u001b[0m Retrieving orders for user: {UserId} with pagination: {@PagedRequest}", userId, pagedRequest);
                var result = await _orderService.GetOrders(userId, pagedRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\u001b[31m[Order]\u001b[0m Error retrieving orders for user: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetail(int orderId)
        {
            try
            {
                int userId = GetUserId();
                _logger.LogInformation("\u001b[32m[Order]\u001b[0m {UserId} is retrieving details for order: {OrderId}", userId, orderId);
                var result = await _orderService.GetOrderDetail(orderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\u001b[31m[Order]\u001b[0m Error retrieving order details for user: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("controller/detail/{orderId}")]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> GetOrderDetailByOrderId(int orderId)
        {
            try
            {
                _logger.LogInformation("\u001b[32m[Order]\u001b[0m Retrieving details for order: {OrderId}", orderId);
                var result = await _orderService.GetOrderDetail(orderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\u001b[31m[Order]\u001b[0m Error retrieving order details: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}