using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlazorEcommerce.Server.Services.PaypalService
{
    public class PaypalService : IPaypalService
    {
        private readonly ICartService _cartService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        public PaypalService(ICartService cartService, IConfiguration configuration, IAuthService authService, IOrderService orderService)
        {
            _cartService = cartService;
            _configuration = configuration;
            _authService = authService;
            _orderService = orderService;
        }

        public async Task<string> MakePaymentPaypalAsync()
        {
            try
            {
                var productServiceResponse = await _cartService.GetDbCartProducts();
                var products = productServiceResponse.Data;

                var total = 0.0m;

                foreach (var product in products)
                {
                    total += product.Price;
                }

                var url = await GetRedirectUrlToPayPalAsync(total, "eur");

                var userId = _authService.GetUserId();

                await _orderService.PlaceOrder(userId, OrderType.Levering);
                return url;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> GetRedirectUrlToPayPalAsync(decimal total, string currency)
        {
            try
            {
                return Task.Run(async () =>
                {
                    var http = GetPaypalHttpClient();

                    // Step 1: Get an access Token
                    var accessToken = await GetPaypalAccessTokenAsync(http);

                    // Step 2: Create the payment
                    var createdPayment = await CreatePaypalPaymentAsync(http, accessToken, total, currency);
                    var approval_url = createdPayment.Links.First(x => x.Rel == "approval_url").Href;

                    return approval_url;
                }).Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private HttpClient GetPaypalHttpClient()
        {
            const string sandbox = "https://api.sandbox.paypal.com";

            var http = new HttpClient()
            {
                BaseAddress = new Uri(sandbox),
                Timeout = TimeSpan.FromSeconds(30),
            };

            return http;
        }

        private async Task<PaypalAccessToken> GetPaypalAccessTokenAsync(HttpClient http)
        {
            var clientId = _configuration["PayPal:clientId"];
            var secret = _configuration["PayPal:secret"];

            var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes($"{clientId}:{secret}");

            var request = new HttpRequestMessage(HttpMethod.Post, "v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

            var form = new Dictionary<string, string>()
            {
                ["grant_type"] = "client_crendentials"
            };

            request.Content = new FormUrlEncodedContent(form);

            var response = await http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var accessToken = JsonConvert.DeserializeObject<PaypalAccessToken>(content);

            return accessToken;
        }

        private static async Task<PaypalCreatedResponse> CreatePaypalPaymentAsync(HttpClient http, PaypalAccessToken accessToken, decimal total, string currency)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "v1/payments/payment");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

            var domain = "https://localhost:7139";

            var payment = JObject.FromObject(new
            {
                intent = "sale",
                redirect_urls = new
                {
                    return_url = $"{domain}/order-success",
                    cancel_url = $"{domain}/cart",
                },
                payer = new { payment_method = "paypal" },
                transactions = JArray.FromObject(new[]
                {
                    new
                    {
                        amount = new
                        {
                            total = total,
                            currency = currency
                        }
                    }
                }),
            });

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            var response = await http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var createdPayment = JsonConvert.DeserializeObject<PaypalCreatedResponse>(content);

            return createdPayment;
        }

        private static async Task<PaypalExecutedResponse> ExecutePaypalPaymentAsync(HttpClient http, PaypalAccessToken accessToken, string paymentId, string payerId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments/payment/{paymentId}/execute");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

            var payment = JObject.FromObject(new
            {
                payer_id = payerId
            });

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            var response = await http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var executedPayment = JsonConvert.DeserializeObject<PaypalExecutedResponse>(content);

            return executedPayment;
        }
    }
}
