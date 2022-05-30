namespace BlazorEcommerce.Client.Pages
{
    public partial class OrderSuccess : ComponentBase
    {
        #region Inject

        [Inject] private ICartService _cartService { get; set; }

        #endregion Inject

        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            await _cartService.GetCartItemsCount();
        }

        #endregion Override Methods
    }
}
