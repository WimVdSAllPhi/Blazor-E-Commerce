namespace BlazorEcommerce.Client.Services.ProductService
{
    public interface IProductService
    {
        // Events
        event Action ProductsChanged;

        // Properties
        List<Product> Products { get; set; }

        List<Product> FeaturedProducts { get; set; }

        List<Product> AdminProducts { get; set; }

        string Message { get; set; }

        int CurrentPage { get; set; }

        int PageCount { get; set; }
        string LastSearchText { get; set; }

        // Methods
        Task GetProductsAsync(string? categoryUrl = null);

        Task<ServiceResponse<Product>> GetProductByIdAsync(int id);

        Task SearchProductsAsync(string searchText, int page);

        Task<List<string>> GetProductSearchSuggestionsAsync(string searchText);

        #region Admin

        Task GetAdminProducts();

        Task<Product> CreateProduct(Product product);

        Task<Product> UpdateProduct(Product product);

        Task DeleteProduct(Product product);

        #endregion Admin
    }
}
