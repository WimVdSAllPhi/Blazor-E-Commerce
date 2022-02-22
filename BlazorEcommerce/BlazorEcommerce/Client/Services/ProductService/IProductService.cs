namespace BlazorEcommerce.Client.Services.ProductService
{
    public interface IProductService
    {
        // Events
        event Action ProductsChanged;

        // Properties
        List<Product> Products { get; set; }

        string Message { get; set; }

        // Methods
        Task GetProductsAsync(string? categoryUrl = null);

        Task<ServiceResponse<Product>> GetProductByIdAsync(int id);

        Task SearchProductsAsync(string searchText);

        Task<List<string>> GetProductSearchSuggestionsAsync(string searchText);
    }
}
