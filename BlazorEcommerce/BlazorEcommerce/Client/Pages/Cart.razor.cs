namespace BlazorEcommerce.Client.Pages
{
    public partial class Cart : ComponentBase
    {
        #region Inject

        [Inject] private ICartService _cartService { get; set; }
        [Inject] private IOrderService _orderService { get; set; }
        [Inject] private IAuthService _authService { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }

        #endregion Inject

        #region Private Properties

        private OrderType _orderType = OrderType.Levering;
        private List<CartProductResponse> _cartProducts = null;
        private string _message = "Loading cart...";
        private bool _isAuthenticated = false;
        private bool _isAddress = false;

        #endregion Private Properties

        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            _isAuthenticated = await _authService.IsUserAuthenticated();
            await LoadCart();
        }

        #endregion Override Methods

        #region Private Methods

        private void AssignAddress(bool isAddress) => _isAddress = isAddress;

        private async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            await _cartService.RemoveProductFromCart(productId, productTypeId);
            await LoadCart();
        }

        private async Task LoadCart()
        {
            await _cartService.GetCartItemsCount();

            _cartProducts = await _cartService.GetCartProducts();

            if (_cartProducts == null || _cartProducts.Count == 0)
            {
                _message = "Your cart is empty.";
            }
        }

        private async Task UpdateQuantity(ChangeEventArgs e, CartProductResponse product)
        {
            product.Quantity = int.Parse(e.Value.ToString());

            if (product.Quantity < 1)
            {
                product.Quantity = 1;
            }

            await _cartService.UpdateQuantity(product);
        }

        private async Task PlaceOrder()
        {
            string url = await _orderService.PlaceOrder(_orderType);
            _navigationManager.NavigateTo(url);
        }

        #endregion Private Methods
    }
}
