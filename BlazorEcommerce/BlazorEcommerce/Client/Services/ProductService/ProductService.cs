namespace BlazorEcommerce.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public event Action ProductsChanged;

        public List<Product> Products { get; set; } = new List<Product>();
        public string Message { get; set; } = "Loading Products...";

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public async Task GetProductsAsync(string? categoryUrl = null)
        {
            var result = categoryUrl == null ?
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product/featured") :
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/category/{categoryUrl}");

            if (result != null && result.Data != null)
            {
                Products = result.Data;
            }

            ProductsChanged.Invoke();
        }

        public async Task<ServiceResponse<Product>> GetProductByIdAsync(int id)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/Product/{id}");

            return result;
        }

        public async Task SearchProductsAsync(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/search/{searchText}");

            if (result != null && result.Data != null)
            {
                Products = result.Data;
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
    }
}
