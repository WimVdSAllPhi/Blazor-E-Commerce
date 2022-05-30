using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaypalService _paypalService;

        public PaymentController(IPaymentService paymentService, IPaypalService paypalService)
        {
            _paymentService = paymentService;
            _paypalService = paypalService;
        }

        [HttpPost("checkout"), Authorize]
        public async Task<ActionResult<string>> CreateCheckoutSession()
        {
            var session = await _paymentService.CreateCheckoutSession();

            var returnUrl = session.Url;

            return Ok(returnUrl);
        }

        [HttpPost("checkoutPayPal"), Authorize]
        public async Task<ActionResult<string>> MakePaymentPaypalAsync()
        {
            var returnUrl = await _paypalService.MakePaymentPaypalAsync();

            return Ok(returnUrl);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> FullfillOrder()
        {
            var response = await _paymentService.FullfillOrder(Request);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }
    }
}
