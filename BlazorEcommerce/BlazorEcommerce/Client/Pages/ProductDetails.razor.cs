namespace BlazorEcommerce.Client.Pages
{
    public partial class ProductDetails : ComponentBase
    {
        #region Parameter

        [Parameter] public int Id { get; set; }

        #endregion Parameter

        #region Inject

        [Inject] private IProductService _productService { get; set; }
        [Inject] private ICartService _cartService { get; set; }
        [Inject] private ToastService _toastService { get; set; }

        #endregion Inject

        #region Private Properties

        private Product? _product = null;
        private string _message = string.Empty;
        private int _currentTypeId = 1;
        private int _quantity = 1;

        #endregion Private Properties

        #region Override Methods

        protected override async Task OnParametersSetAsync()
        {
            _message = string.Empty;

            var result = await _productService.GetProductByIdAsync(Id);

            if (!result.Success)
            {
                _message = result.Message;
            }
            else
            {
                _product = result.Data;

                if (_product.Variants.Count > 0)
                {
                    _currentTypeId = _product.Variants[0].ProductTypeId;
                }
            }
        }

        #endregion Override Methods

        #region Private Methods

        private void AssignQuantity(int quantity) => _quantity = quantity;

        private ProductVariant GetSelectedVariant()
        {
            var variant = _product.Variants.FirstOrDefault(x => x.ProductTypeId == _currentTypeId);

            return variant;
        }

        private async Task AddToCart()
        {
            var productVariant = GetSelectedVariant();

            var cartItem = new CartItem()
            {
                ProductId = productVariant.ProductId,
                ProductTypeId = productVariant.ProductTypeId,
                Quantity = _quantity,
            };

            await _cartService.AddToCart(cartItem);

            _toastService.ShowToast("Added to cart.", ToastLevel.Success, 2000);
        }

        #endregion Private Methods
    }
}
