namespace BlazorEcommerce.Client.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public partial class AdList : ComponentBase
    {
        #region Inject

        [Inject] private IAdService? _adService { get; set; }
        [Inject] private ICategoryService? _categoryService { get; set; }

        #endregion Inject

        #region Private Properties

        private Ad? _ad = null;

        #endregion Private Properties

        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            _ad = new Ad();

            if (_categoryService != null)
            {
                await _categoryService.GetCategories();
            }
        }

        #endregion Override Methods

        #region Private Methods

        private void AssignImageUrl(string imgUrl) => _ad.ImageUrl = imgUrl;

        private async Task AddAsync()
        {
            if (_adService != null && _ad != null)
            {
                var result = await _adService.CreateAd(_ad);

                _ad = result;

                await _adService.GetAdsAsync();
            }
        }

        #endregion Private Methods
    }
}
