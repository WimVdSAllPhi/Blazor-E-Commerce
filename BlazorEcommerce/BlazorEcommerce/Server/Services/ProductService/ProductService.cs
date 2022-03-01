namespace BlazorEcommerce.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Admin

        public async Task<ServiceResponse<List<Product>>> GetAdminProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>();

            var products = await _context.Products
                .Where(x => !x.Deleted)
                .Include(x => x.Variants.Where(x => !x.Deleted))
                .ThenInclude(x => x.ProductType)
                .ToListAsync();

            response.Data = products;

            return response;
        }

        public async Task<ServiceResponse<Product>> CreateProductAsync(Product product)
        {
            foreach (var variant in product.Variants)
            {
                variant.ProductType = null;
            }

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            var response = new ServiceResponse<Product>
            {
                Data = product
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> UpdateProductAsync(Product product)
        {
            var dbProduct = await _context.Products.FindAsync(product.Id);

            if (dbProduct == null)
            {
                var errorResponse = new ServiceResponse<Product>
                {
                    Success = false,
                    Message = "Product not found."
                };

                return errorResponse;
            }

            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Visible = product.Visible;
            dbProduct.Featured = product.Featured;

            foreach (var variant in product.Variants)
            {
                var dbVariant = await _context.ProductVariants
                    .SingleOrDefaultAsync(v => v.ProductId == variant.ProductId && v.ProductTypeId == variant.ProductTypeId);

                if (dbVariant == null)
                {
                    variant.ProductType = null;

                    _context.ProductVariants.Add(variant);
                }
                else
                {
                    dbVariant.ProductTypeId = variant.ProductTypeId;
                    dbVariant.Price = variant.Price;
                    dbVariant.OriginalPrice = variant.OriginalPrice;
                    dbVariant.Visible = variant.Visible;
                    dbVariant.Deleted = variant.Deleted;
                }
            }

            await _context.SaveChangesAsync();

            var response = new ServiceResponse<Product>
            {
                Data = product
            };

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteProductAsync(int productId)
        {
            var dbProduct = await _context.Products.FindAsync(productId);

            if (dbProduct == null)
            {
                var errorResponse = new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Product not found."
                };

                return errorResponse;
            }

            dbProduct.Deleted = true;

            await _context.SaveChangesAsync();

            var response = new ServiceResponse<bool>
            {
                Data = true
            };

            return response;
        }

        #endregion Admin

        public async Task<ServiceResponse<List<Product>>> GetAllProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>();

            var products = await _context.Products
                .Where(x => x.Visible && !x.Deleted)
                .Include(x => x.Variants.Where(x => x.Visible && !x.Deleted))
                .ToListAsync();

            response.Data = products;

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>();

            var products = await _context.Products
                .Where(x => x.Featured && x.Visible && !x.Deleted)
                .Include(x => x.Variants.Where(x => x.Visible && !x.Deleted))
                .ToListAsync();

            response.Data = products;

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductByIdAsync(int id)
        {
            var response = new ServiceResponse<Product>();

            Product product = null;

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                product = await _context.Products
                    .Include(x => x.Variants.Where(x => !x.Deleted))
                    .ThenInclude(x => x.ProductType)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
            }
            else
            {
                product = await _context.Products
                    .Include(x => x.Variants.Where(x => x.Visible && !x.Deleted))
                    .ThenInclude(x => x.ProductType)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted && x.Visible);
            }

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
                .Where(x => x.Category.Url.ToLower().Equals(categoryUrl.ToLower()) && x.Visible && !x.Deleted)
                .Include(x => x.Variants.Where(x => x.Visible && !x.Deleted))
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

        public async Task<ServiceResponse<ProductSearchResult>> SearchProductsAsync(string searchText, int page)
        {
            List<Product> products = await FindProductsBySearchTextAsync(searchText);

            var pageResults = 2f;
            var pageCount = Math.Ceiling(products.Count / pageResults);

            var productsPaged = await _context.Products
                .Where(x => x.Title.ToLower().Contains(searchText.ToLower())
                    ||
                    x.Description.ToLower().Contains(searchText.ToLower())
                    && x.Visible && !x.Deleted)
                .Include(x => x.Variants.Where(x => x.Visible && !x.Deleted))
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var response = new ServiceResponse<ProductSearchResult>
            {
                Data = new ProductSearchResult()
                {
                    Products = productsPaged,
                    CurrentPage = page,
                    Pages = (int)pageCount,
                }
            };

            return response;
        }

        private async Task<List<Product>> FindProductsBySearchTextAsync(string searchText)
        {
            return await _context.Products
                .Where(x => x.Title.ToLower().Contains(searchText.ToLower())
                    ||
                    x.Description.ToLower().Contains(searchText.ToLower())
                    && x.Visible && !x.Deleted)
                .Include(x => x.Variants.Where(x => x.Visible && !x.Deleted))
                .ToListAsync();
        }
    }
}
