using System.Security.Claims;

namespace BlazorEcommerce.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _authStateProvider = authStateProvider;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/change-password", request.Password);

            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return content;
        }

        public async Task<ServiceResponse<string>> Login(UserLogin request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/login", request);

            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();

            return content;
        }

        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/register", request);

            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            return content;
        }

        public async Task<ServiceResponse<UserProfile>> GetProfileAsync()
        {
            var id = await GetUserId();
            var result = await _http.GetFromJsonAsync<ServiceResponse<UserProfile>>($"api/Auth/{id}");

            return result;
        }

        public async Task<int> GetUserId()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();

            var claim = state.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var claimValue = claim.Value;

                if (int.TryParse(claimValue, out var id))
                {
                    return id;
                }
            }

            return 0;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();

            var isAuthenticated = state.User.Identity.IsAuthenticated;

            return isAuthenticated;
        }

        public async Task<bool> IsAdminOrNotAsync()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();

            if (state != null)
            {
                var user = state.User;

                if (user != null)
                {
                    var claims = user.Claims;

                    if (claims != null && claims.Count() > 0)
                    {
                        var claim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                        if (claim != null)
                        {
                            var claimValue = claim.Value;

                            if (claimValue.Contains("Admin"))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public async Task<ServiceResponse<UserProfile>> UpdateUser(UserProfile user)
        {
            var result = await _http.PutAsJsonAsync("api/auth", user);

            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<UserProfile>>();

            return content;
        }
    }
}
