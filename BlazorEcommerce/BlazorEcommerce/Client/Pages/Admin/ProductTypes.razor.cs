namespace BlazorEcommerce.Client.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public partial class ProductTypes : ComponentBase, IDisposable
    {
        #region Inject

        [Inject] private IProductTypeService _productTypeService { get; set; }

        #endregion Inject

        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            await _productTypeService.GetProductTypes();
            _productTypeService.OnChange += StateHasChanged;
        }

        #endregion Override Methods

        #region Private Methods

        private void CreateNewProductType()
        {
            var editingProductType = _productTypeService.CreateNewProductType();
            editingProductType.Editing = true;
            editingProductType.IsNew = true;
        }

        private void EditProductType(ProductType productType)
        {
            if (productType.IsNew)
            {
                _productTypeService.ProductTypes.Remove(productType);
            }
            else
            {
                productType.Editing = true;
            }
        }

        private async Task UpdateProductType(ProductType productType)
        {
            if (productType.IsNew)
                await _productTypeService.AddProductType(productType);
            else
                await _productTypeService.UpdateProductType(productType);
        }

        private async Task CancelEditing()
        {
            await _productTypeService.GetProductTypes();
        }

        private async Task DeleteProductType(int id)
        {
            await _productTypeService.DeleteProductType(id);
        }

        private void UpdateProductTypeName(ChangeEventArgs e, ProductType productType)
        {
            if (e.Value != null)
            {
                productType.Name = e.Value.ToString();
            }
        }

        public void Dispose()
        {
            _productTypeService.OnChange -= StateHasChanged;
        }

        #endregion Private Methods
    }
}
