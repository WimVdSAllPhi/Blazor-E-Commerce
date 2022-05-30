namespace BlazorEcommerce.Client.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public partial class Products : ComponentBase
    {
        #region Inject

        [Inject] private IProductService _productService { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private IJSRuntime _jSRuntime { get; set; }

        #endregion Inject

        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            await _productService.GetAdminProducts();
        }

        #endregion Override Methods

        #region Private Methods

        private string IsVisible(Product product) => product.Visible ? "oi-check text-success" : "oi-x text-danger";

        private string IsFeatured(Product product) => product.Featured ? "oi-check text-success" : "oi-x text-danger";

        private void EditProduct(int productId)
        {
            _navigationManager.NavigateTo($"admin/product/{productId}");
        }

        private void CreateProduct()
        {
            _navigationManager.NavigateTo("admin/product");
        }

        private async void DeleteProductAsync(Product product)
        {
            var confirmed = await _jSRuntime.InvokeAsync<bool>("confirm", $"Do you really want to delete '{product.Title}'?");

            if (confirmed)
            {
                await _productService.DeleteProduct(product);

                _navigationManager.NavigateTo("admin/products");
            }
        }

        #endregion Private Methods
    }
}
