namespace BlazorEcommerce.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Product>>> GetAllProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>();

            var products = await _context.Products
                .Include(x => x.Variants)
                .ToListAsync();

            response.Data = products;

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductByIdAsync(int id)
        {
            var response = new ServiceResponse<Product>();

            var product = await _context.Products
                .Include(x => x.Variants)
                .ThenInclude(x => x.ProductType)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this product does not exist.";
            }
            else
            {
                response.Data = product;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>();

            var products = await _context.Products
                .Where(x => x.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
                .Include(x => x.Variants)
                .ToListAsync();

            response.Data = products;

            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestionsAsync(string searchText)
        {
            var products = await FindProductsBySearchTextAsync(searchText);

            var result = new List<string>();

            var response = new ServiceResponse<List<string>>();

            foreach (var product in products)
            {
                if (product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Title);
                }

                if (product.Description != null)
                {
                    var punctuation = product.Description.Where(char.IsPunctuation).Distinct().ToArray();

                    var words = product.Description.Split().Select(s => s.Trim(punctuation));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }
            }

            response.Data = result;

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> SearchProductsAsync(string searchText)
        {
            var response = new ServiceResponse<List<Product>>();
            List<Product> products = await FindProductsBySearchTextAsync(searchText);

            response.Data = products;

            return response;
        }

        private async Task<List<Product>> FindProductsBySearchTextAsync(string searchText)
        {
            return await _context.Products
                .Where(x => x.Title.ToLower().Contains(searchText.ToLower())
                    ||
                    x.Description.ToLower().Contains(searchText.ToLower()))
                .Include(x => x.Variants)
                .ToListAsync();
        }
    }
}
