namespace BlazorEcommerce.Client.Pages
{
    public partial class Register : ComponentBase
    {
        #region Inject

        [Inject] private IAuthService AuthService { get; set; }
        [Inject] private ToastService ToastService { get; set; }

        #endregion Inject

        #region Private Properties

        private UserRegister user = new UserRegister();
        private string message = string.Empty;
        private string messageCssClass = string.Empty;

        #endregion Private Properties

        #region Private Methods

        private void AssignImageUrl(string imgUrl) => user.ImageUrl = imgUrl;

        private async Task HandleRegistration()
        {
            var result = await AuthService.Register(user);

            message = result.Message;

            if (result.Success)
            {
                ToastService.ShowToast(message, ToastLevel.Success);

                messageCssClass = "text-success";
                message += $" go to <a href=\"login\">Login</a>";
            }
            else
                messageCssClass = "text-danger";
        }

        #endregion Private Methods
    }
}
