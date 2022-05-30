namespace BlazorEcommerce.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public event Action ProductsChanged;

        public List<Product> Products { get; set; }
        public List<Product> FeaturedProducts { get; set; }
        public string Message { get; set; } = "Loading Products...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;
        public List<Product> AdminProducts { get; set; }

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public async Task GetProductsAsync(string? categoryUrl = null)
        {
            FeaturedProducts = null;
            Products = null;

            var result = categoryUrl == null ?
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product/featured") :
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/category/{categoryUrl}");

            if (result != null && result.Data != null)
            {
                if (categoryUrl == null)
                {
                    FeaturedProducts = result.Data;
                }
                else
                {
                    Products = result.Data;

                    CurrentPage = 1;
                    PageCount = 0;

                    if (Products.Count == 0)
                        Message = "No Products found.";
                }
            }

            ProductsChanged.Invoke();
        }

        public async Task<ServiceResponse<Product>> GetProductByIdAsync(int id)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/Product/{id}");

            return result;
        }

        public async Task SearchProductsAsync(string searchText, int page)
        {
            LastSearchText = searchText;

            var result = await _http.GetFromJsonAsync<ServiceResponse<ProductSearchResult>>($"api/Product/search/{searchText}/{page}");

            if (result != null && result.Data != null)
            {
                Products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            if (Products.Count == 0)
            {
                Message = "No products found.";
            }

            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestionsAsync(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/Product/searchsuggestions/{searchText}");

            return result.Data;
        }

        #region Admin

        public async Task GetAdminProducts()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product/admin");

            AdminProducts = result.Data;

            CurrentPage = 1;
            PageCount = 0;

            if (AdminProducts.Count == 0)
            {
                Message = "No products found.";
            }
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var result = await _http.PostAsJsonAsync("api/product", product);

            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>();

            return content.Data;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var result = await _http.PutAsJsonAsync("api/product", product);

            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>();

            return content.Data;
        }

        public async Task DeleteProduct(Product product)
        {
            var result = await _http.DeleteAsync($"api/product/{product.Id}");
        }

        #endregion Admin
    }
}
