namespace BlazorEcommerce.Client.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public partial class Categories : ComponentBase, IDisposable
    {
        #region Inject

        [Inject] private ICategoryService? _categoryService { get; set; }

        #endregion Inject

        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            if (_categoryService != null)
            {
                await _categoryService.GetAdminCategories();
                _categoryService.OnChange += StateHasChanged;
            }
        }

        #endregion Override Methods

        #region Private Methods

        private void CreateNewCategory()
        {
            if (_categoryService != null)
            {
                var editingCategory = _categoryService.CreateNewCategory();
                editingCategory.Editing = true;
                editingCategory.IsNew = true;
            }
        }

        private void EditCategory(Category category)
        {
            if (_categoryService != null)
            {
                if (category.IsNew)
                {
                    _categoryService.AdminCategories.Remove(category);
                }
                else
                {
                    category.Editing = true;
                }
            }
        }

        private async Task UpdateCategory(Category category)
        {
            if (_categoryService != null)
            {
                if (category.IsNew)
                    await _categoryService.AddCategory(category);
                else
                    await _categoryService.UpdateCategory(category);
            }
        }

        private async Task CancelEditing()
        {
            if (_categoryService != null)
            {
                await _categoryService.GetAdminCategories();
            }
        }

        private async Task DeleteCategory(int id)
        {
            if (_categoryService != null)
            {
                await _categoryService.DeleteCategory(id);
            }
        }

        private void UpdateCategoryName(ChangeEventArgs e, Category category)
        {
            if (e != null && e.Value != null)
            {
                category.Name = e.Value.ToString();
            }
        }

        private void UpdateCategoryUrl(ChangeEventArgs e, Category category)
        {
            if (e != null && e.Value != null)
            {
                category.Url = e.Value.ToString();
            }
        }

        private void UpdateCategoryVisible(ChangeEventArgs e, Category category)
        {
            if (e != null && e.Value != null)
            {
                category.Visible = bool.Parse(e.Value.ToString());
            }
        }

        public void Dispose()
        {
            if (_categoryService != null)
            {
                _categoryService.OnChange -= StateHasChanged;
            }
        }

        #endregion Private Methods
    }
}
