using Microsoft.AspNetCore.Authorization;

namespace BlazorEcommerce.Client.Pages
{
    [Authorize]
    public partial class Profile : ComponentBase
    {
        #region Inject

        [Inject] private IAuthService _authService { get; set; }

        #endregion Inject

        #region Private Properties

        private UserProfile _user;
        private string _errorMessage = string.Empty;
        private UserChangePassword _request = new UserChangePassword();
        private string _message = string.Empty;
        private bool _isEdit = false;
        private bool _isPasswordEdit = false;

        #endregion Private Properties

        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            var result = await _authService.GetProfileAsync();

            if (result.Success)
            {
                _user = result.Data;
                _errorMessage = string.Empty;
            }
            else
            {
                _user = new UserProfile();
                _errorMessage = result.Message;
            }
        }

        #endregion Override Methods

        #region Private Methods

        private void AssignImageUrl(string imgUrl) => _user.ImageUrl = imgUrl;

        private async Task ChangePassword()
        {
            var result = await _authService.ChangePassword(_request);
            _message = result.Message;

            _isPasswordEdit = false;
        }

        private async Task UpdateUser()
        {
            var result = await _authService.UpdateUser(_user);

            if (result.Success)
            {
                _user = result.Data;
            }
            else
            {
                _message = result.Message;
            }

            _isEdit = false;
        }

        private void Edit()
        {
            _isEdit = true;
        }

        private void CancelEdit()
        {
            _isEdit = false;
        }

        private void EditPassword()
        {
            _isPasswordEdit = true;
        }

        private void CancelPasswordEdit()
        {
            _isPasswordEdit = false;
        }

        #endregion Private Methods
    }
}
