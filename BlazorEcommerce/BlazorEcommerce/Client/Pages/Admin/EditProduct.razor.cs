namespace BlazorEcommerce.Client.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public partial class EditProduct : ComponentBase
    {
        #region Parameter

        [Parameter] public int? Id { get; set; }

        #endregion Parameter

        #region Inject

        [Inject] private IProductService? _productService { get; set; }
        [Inject] private IProductTypeService? _productTypeService { get; set; }
        [Inject] private ICategoryService? _categoryService { get; set; }
        [Inject] private NavigationManager? _navigationManager { get; set; }
        [Inject] private IJSRuntime? _jSRuntime { get; set; }

        #endregion Inject

        #region Private Properties

        private Product? _product = null;
        private bool _isUpdate = false;
        private string _errorMessage = string.Empty;

        #endregion Private Properties

        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            if (_productTypeService != null)
            {
                await _productTypeService.GetProductTypes();
            }

            if (_categoryService != null)
            {
                await _categoryService.GetAdminCategories();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!Id.HasValue || Id == 0)
            {
                _product = new Product { IsNew = true };

                _isUpdate = false;
            }
            else
            {
                if (_productService != null)
                {
                    var serviceResponseProduct = await _productService.GetProductByIdAsync(Id.Value);

                    var dbProduct = serviceResponseProduct.Data;

                    if (dbProduct == null)
                    {
                        _product = new Product { IsNew = true };

                        _isUpdate = false;

                        _errorMessage = $"The product with id: {Id} does not exist.";
                    }
                    else
                    {
                        _product = dbProduct;
                        _product.Editing = true;

                        _isUpdate = true;
                    }
                }
            }
        }

        #endregion Override Methods

        #region Private Methods

        private void AssignImageUrl(List<string> productImages)
        {
            _product.ProductImages = new List<ProductImage>();

            foreach (var imageUrl in productImages)
            {
                _product.ProductImages.Add(new ProductImage()
                {
                    ImageUrl = imageUrl,
                });
            }
        }

        private void RemoveVariant(int productTypeId)
        {
            var variant = _product.Variants.Find(v => v.ProductTypeId == productTypeId);

            if (variant == null)
            {
                return;
            }

            if (variant.IsNew)
            {
                _product.Variants.Remove(variant);
            }
            else
            {
                variant.Deleted = true;
            }
        }

        private void AddVariant()
        {
            _product.Variants.Add(new ProductVariant { IsNew = true, ProductId = _product.Id });
        }

        private async void AddOrUpdateProductAsync()
        {
            if (_product.IsNew)
            {
                var result = await _productService.CreateProduct(_product);
            }
            else
            {
                _product.IsNew = false;
                _product = await _productService.UpdateProduct(_product);
            }

            _navigationManager.NavigateTo($"admin/products");
        }

        private async void DeleteProductAsync()
        {
            var confirmed = await _jSRuntime.InvokeAsync<bool>("confirm", $"Do you really want to delete '{_product.Title}'?");

            if (confirmed)
            {
                await _productService.DeleteProduct(_product);

                _navigationManager.NavigateTo("admin/products");
            }
        }

        #endregion Private Methods
    }
}
