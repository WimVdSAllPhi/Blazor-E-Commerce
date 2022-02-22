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

            var products = await _context.Products.ToListAsync();

            response.Data = products;

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductByIdAsync(int id)
        {
            var response = new ServiceResponse<Product>();

            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);

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
                .ToListAsync();

            response.Data = products;

            return response;
        }
    }
}
