namespace BlazorEcommerce.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Category>>> AddCategory(Category category)
        {
            category.Editing = category.IsNew = false;

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            var categories = await GetAdminCategoriesAsync();

            return categories;
        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategory(int id)
        {
            var category = await GetCategoryById(id);

            if (category == null)
            {
                var errorResponse = new ServiceResponse<List<Category>>()
                {
                    Success = false,
                    Message = "Category not found."
                };

                return errorResponse;
            }

            category.Deleted = true;

            await _context.SaveChangesAsync();

            var categories = await GetAdminCategoriesAsync();

            return categories;
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync()
        {
            var response = new ServiceResponse<List<Category>>();

            var categories = await _context.Categories.Where(x => !x.Deleted).ToListAsync();

            response.Data = categories;

            return response;
        }

        public async Task<ServiceResponse<List<Category>>> GetAllCategoriesAsync()
        {
            var response = new ServiceResponse<List<Category>>();

            var categories = await _context.Categories.Where(x => !x.Deleted && x.Visible).ToListAsync();

            response.Data = categories;

            return response;
        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategory(Category category)
        {
            var dbCategory = await GetCategoryById(category.Id);

            if (dbCategory == null)
            {
                var errorResponse = new ServiceResponse<List<Category>>()
                {
                    Success = false,
                    Message = "Category not found."
                };

                return errorResponse;
            }

            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;

            await _context.SaveChangesAsync();

            var categories = await GetAdminCategoriesAsync();

            return categories;
        }

        private async Task<Category> GetCategoryById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            return category;
        }
    }
}
