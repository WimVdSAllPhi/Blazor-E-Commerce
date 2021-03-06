namespace BlazorEcommerce.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;

        public CartService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            var userId = _authService.GetUserId();
            cartItem.UserId = userId;

            var sameItem = await _context.CartItems
                .FirstOrDefaultAsync(x =>
                x.ProductId == cartItem.ProductId &&
                x.ProductTypeId == cartItem.ProductTypeId &&
                x.UserId == cartItem.UserId);

            if (sameItem == null)
            {
                _context.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await _context.SaveChangesAsync();

            var response = new ServiceResponse<bool>()
            {
                Data = true
            };

            return response;
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var userId = _authService.GetUserId();
            var count = await _context.CartItems.Where(x => x.UserId == userId).CountAsync();

            var response = new ServiceResponse<int>()
            {
                Data = count
            };

            return response;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProductsAsync(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResponse>>();

            var list = new List<CartProductResponse>();

            foreach (var cartItem in cartItems)
            {
                var product = await _context.Products.Where(x => x.Id == cartItem.ProductId).Include(x => x.ProductImages).FirstOrDefaultAsync();

                if (product == null)
                {
                    continue;
                }

                var productVariant = await _context.ProductVariants
                    .Where(x => x.ProductId == cartItem.ProductId && x.ProductTypeId == cartItem.ProductTypeId)
                    .Include(x => x.ProductType)
                    .FirstOrDefaultAsync();

                if (productVariant == null)
                {
                    continue;
                }

                var cartProductResponse = new CartProductResponse
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ProductImages = product.ProductImages,
                    Price = productVariant.Price,
                    ProductTypeId = productVariant.ProductTypeId,
                    ProductTypeName = productVariant.ProductType.Name,
                    Quantity = cartItem.Quantity,
                };

                list.Add(cartProductResponse);
            }

            result.Data = list;

            return result;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts(int? userId = null)
        {
            if (userId == null)
            {
                userId = _authService.GetUserId();
            }

            var list = await _context.CartItems.Where(x => x.UserId == userId).ToListAsync();
            var response = await GetCartProductsAsync(list);

            return response;
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var userId = _authService.GetUserId();

            var dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(x =>
                x.ProductId == productId &&
                x.ProductTypeId == productTypeId &&
                x.UserId == userId);

            if (dbCartItem == null)
            {
                var errorResponse = new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Message = "Cart item does not exist."
                };

                return errorResponse;
            }

            _context.CartItems.Remove(dbCartItem);

            await _context.SaveChangesAsync();

            var response = new ServiceResponse<bool>()
            {
                Data = true,
            };

            return response;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems)
        {
            var userId = _authService.GetUserId();

            cartItems.ForEach(cartItem => cartItem.UserId = userId);

            await _context.CartItems.AddRangeAsync(cartItems);

            await _context.SaveChangesAsync();

            var response = await GetDbCartProducts();

            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            var userId = _authService.GetUserId();

            var dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(x =>
                x.ProductId == cartItem.ProductId &&
                x.ProductTypeId == cartItem.ProductTypeId &&
                x.UserId == userId);

            if (dbCartItem == null)
            {
                var errorResponse = new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Message = "Cart item does not exist."
                };

                return errorResponse;
            }

            dbCartItem.Quantity = cartItem.Quantity;
            await _context.SaveChangesAsync();

            var response = new ServiceResponse<bool>()
            {
                Data = true,
            };

            return response;
        }
    }
}
