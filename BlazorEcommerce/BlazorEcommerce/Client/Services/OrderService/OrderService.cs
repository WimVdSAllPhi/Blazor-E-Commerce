using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;

        public OrderService(
            HttpClient http,
            AuthenticationStateProvider authStateProvider,
            NavigationManager navigationManager)
        {
            _http = http;
            _authStateProvider = authStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<OrderDetailsResponse> GetOrderDetails(int orderId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsResponse>>($"api/order/{orderId}");

            return result.Data;
        }

        public async Task<List<OrderOverviewResponse>> GetOrders()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<OrderOverviewResponse>>>("api/order");

            return result.Data;
        }

        public async Task<string> PlaceOrder()
        {
            var isAuthenticated = await IsUserAuthenticated();

            if (isAuthenticated)
            {
                var result = await _http.PostAsync("api/payment/checkout", null);
                var url = await result.Content.ReadAsStringAsync();

                return url;
            }
            else
            {
                return "login";
            }
        }

        private async Task<bool> IsUserAuthenticated()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var isAuthenticated = state.User.Identity.IsAuthenticated;
            return isAuthenticated;
        }
    }
}
