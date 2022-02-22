namespace BlazorEcommerce.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetAllProductsAsync();

        Task<ServiceResponse<Product>> GetProductByIdAsync(int id);

        Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl);

        Task<ServiceResponse<List<Product>>> SearchProductsAsync(string searchText);

        Task<ServiceResponse<List<string>>> GetProductSearchSuggestionsAsync(string searchText);

        Task<ServiceResponse<List<Product>>> GetFeaturedProductsAsync();
    }
}
