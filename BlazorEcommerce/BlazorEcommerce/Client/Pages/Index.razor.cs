namespace BlazorEcommerce.Client.Pages
{
    public partial class Index : ComponentBase
    {
        #region Parameter

        [Parameter] public string? CategoryUrl { get; set; } = null;

        [Parameter] public string? SearchText { get; set; } = null;

        [Parameter] public int Page { get; set; } = 1;

        #endregion Parameter

        #region Inject

        [Inject] private IProductService? _productService { get; set; }

        #endregion Inject

        #region Override Methods

        protected override async Task OnParametersSetAsync()
        {
            if (SearchText != null)
            {
                if (_productService != null)
                {
                    await _productService.SearchProductsAsync(SearchText, Page);
                }
            }
        }

        #endregion Override Methods
    }
}
