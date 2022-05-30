using Stripe;
using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        private const string secret = "whsec_be4c2f5c6d508acbaecac18effa03378d892ea20dd43419a7085c7415948fcc9";

        public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService)
        {
            // Secret Key of Stripe
            StripeConfiguration.ApiKey = "sk_test_51KWhUBDPMXVtxParSAJGMCNt3Jt4LgLgSZgsPn9iC4stROT7ckZLujezvUM7VvF6UIIKAZHl64KW9HYZuGorbD9u00qVRXP6OV";

            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
        }

        public async Task<Session> CreateCheckoutSession()
        {
            var productServiceResponse = await _cartService.GetDbCartProducts();

            var products = productServiceResponse.Data;

            var lineItems = new List<SessionLineItemOptions>();

            products.ForEach(product => lineItems.Add(new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = product.Title,
                        Images = product.ProductImages.Select(x => x.ImageUrl).ToList(),
                    },
                },
                Quantity = product.Quantity,
            }));

            var email = _authService.GetUserEmail();

            var domain = "https://localhost:7139";

            var options = new SessionCreateOptions()
            {
                CustomerEmail = email,
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions()
                {
                    AllowedCountries = new List<string>() {
                        "BE"
                    },
                },
                PaymentMethodTypes = new List<string>()
                {
                    "card",
                    "ideal",
                    "bancontact"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = $"{domain}/order-success",
                CancelUrl = $"{domain}/cart"
            };

            var service = new SessionService();
            var session = service.Create(options);

            return session;
        }

        public async Task<ServiceResponse<bool>> FullfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, request.Headers["Stripe-Signature"], secret);

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;

                    var user = await _authService.GetUserByEmail(session.CustomerEmail);

                    await _orderService.PlaceOrder(user.Id, OrderType.Levering);
                }

                return new ServiceResponse<bool>() { Data = true };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>() { Data = false, Success = false, Message = ex.Message };
            }
        }
    }
}
