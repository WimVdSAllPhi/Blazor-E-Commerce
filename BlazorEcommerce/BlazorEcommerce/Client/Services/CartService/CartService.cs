using Blazored.LocalStorage;

namespace BlazorEcommerce.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly IAuthService _authService;

        public event Action OnChange;

        public CartService(ILocalStorageService localStorage, HttpClient http, IAuthService authService)
        {
            _localStorage = localStorage;
            _http = http;
            _authService = authService;
        }

        public async Task AddToCart(CartItem cartItem)
        {
            var isAuthenticated = await _authService.IsUserAuthenticated();

            if (isAuthenticated)
            {
                await _http.PostAsJsonAsync("api/cart/add", cartItem);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

                if (cart == null)
                {
                    cart = new List<CartItem>();
                }

                var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId && x.ProductTypeId == cartItem.ProductTypeId);

                if (sameItem == null)
                {
                    cart.Add(cartItem);
                }
                else
                {
                    sameItem.Quantity += cartItem.Quantity;
                }

                await _localStorage.SetItemAsync("cart", cart);
            }

            await GetCartItemsCount();
        }

        public async Task<List<CartProductResponse>> GetCartProducts()
        {
            var isAuthenticated = await _authService.IsUserAuthenticated();

            if (isAuthenticated)
            {
                var response = await _http.GetFromJsonAsync<ServiceResponse<List<CartProductResponse>>>("api/cart");

                return response.Data;
            }
            else
            {
                var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");

                if (cartItems == null)
                {
                    return new List<CartProductResponse>();
                }
                else
                {
                    var result = await _http.PostAsJsonAsync("api/cart/products", cartItems);

                    var content = await result.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();

                    return content.Data;
                }
            }
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            var isAuthenticated = await _authService.IsUserAuthenticated();

            if (isAuthenticated)
            {
                await _http.DeleteAsync($"api/cart/{productId}/{productTypeId}");
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);

                if (cartItem != null)
                {
                    cart.Remove(cartItem);

                    await _localStorage.SetItemAsync("cart", cart);
                }
            }
        }

        public async Task UpdateQuantity(CartProductResponse product)
        {
            var isAuthenticated = await _authService.IsUserAuthenticated();

            if (isAuthenticated)
            {
                var request = new CartItem
                {
                    ProductId = product.ProductId,
                    ProductTypeId = product.ProductTypeId,
                    Quantity = product.Quantity,
                };

                await _http.PutAsJsonAsync("api/cart/update-quantity", request);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(x => x.ProductId == product.ProductId && x.ProductTypeId == product.ProductTypeId);

                if (cartItem != null)
                {
                    cartItem.Quantity = product.Quantity;

                    await _localStorage.SetItemAsync("cart", cart);
                }
            }
        }

        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            var isAuthenticated = await _authService.IsUserAuthenticated();

            if (isAuthenticated)
            {
                var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

                if (localCart == null)
                {
                    return;
                }

                await _http.PostAsJsonAsync("api/cart", localCart);

                if (emptyLocalCart)
                {
                    await _localStorage.RemoveItemAsync("cart");
                }
            }
        }

        public async Task GetCartItemsCount()
        {
            var isAuthenticated = await _authService.IsUserAuthenticated();

            if (isAuthenticated)
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = result.Data;

                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

                if (cart != null)
                {
                    await _localStorage.SetItemAsync<int>("cartItemsCount", cart.Count);
                }
                else
                {
                    await _localStorage.SetItemAsync<int>("cartItemsCount", 0);
                }
            }

            OnChange.Invoke();
        }
    }
}
