namespace BlazorEcommerce.Client.Shared.Elements
{
    public partial class FeaturedProducts : ComponentBase, IDisposable
    {
        #region Inject

        [Inject] private IProductService? _productService { get; set; }

        #endregion Inject

        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            if (_productService != null)
            {
                _productService.ProductsChanged += StateHasChanged;

                await _productService.GetProductsAsync();
            }
        }

        #endregion Override Methods

        #region Private Methods

        public void Dispose()
        {
            if (_productService != null)
            {
                _productService.ProductsChanged -= StateHasChanged;
            }
        }

        #endregion Private Methods
    }
}
