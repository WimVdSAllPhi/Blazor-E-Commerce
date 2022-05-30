using Microsoft.AspNetCore.WebUtilities;

namespace BlazorEcommerce.Client.Pages
{
    public partial class Login : ComponentBase
    {
        #region Inject

        [Inject] private IAuthService _authService { get; set; }
        [Inject] private ILocalStorageService _localStorage { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; }
        [Inject] private ICartService _cartService { get; set; }

        #endregion Inject

        #region Private Properties

        private UserLogin _user = new UserLogin();
        private string _errorMessage = string.Empty;
        private string _returnUrl = string.Empty;

        #endregion Private Properties

        #region Override Methods

        protected override void OnInitialized()
        {
            var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out var url))
            {
                _returnUrl = url;
            }
        }

        #endregion Override Methods

        #region Private Methods

        private async Task HandleLogin()
        {
            var result = await _authService.Login(_user);

            if (result.Success)
            {
                _errorMessage = string.Empty;

                await _localStorage.SetItemAsync("authToken", result.Data);

                await _cartService.StoreCartItems(true);

                await _cartService.GetCartItemsCount();

                _navigationManager.NavigateTo(_returnUrl, true);
            }
            else
            {
                _errorMessage = result.Message;
            }
        }

        #endregion Private Methods
    }
}
