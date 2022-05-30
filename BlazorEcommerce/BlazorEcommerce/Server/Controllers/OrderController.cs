using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IAuthService _authService;

        public OrderController(IOrderService orderService, IAuthService authService)
        {
            _orderService = orderService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewResponse>>>> GetOrders()
        {
            var result = await _orderService.GetOrders();

            return Ok(result);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<OrderDetailsResponse>>> GetOrderDetails(int orderId)
        {
            var result = await _orderService.GetOrderDetails(orderId);

            return Ok(result);
        }

        [HttpGet("place/{orderType}"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder(OrderType orderType)
        {
            var userId = _authService.GetUserId();

            var result = await _orderService.PlaceOrder(userId, orderType);

            return Ok(result);
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<OrderAdmin>>>> GetAllOrders()
        {
            var result = await _orderService.GetAllOrders();

            return Ok(result);
        }

        [HttpPut("admin/{orderId}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<OrderAdmin>>>> PutAsDone(int orderId)
        {
            var result = await _orderService.PutAsDone(orderId);
            return Ok(result);
        }
    }
}
