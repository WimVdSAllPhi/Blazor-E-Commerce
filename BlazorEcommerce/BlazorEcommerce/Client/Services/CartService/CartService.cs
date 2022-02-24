using Blazored.LocalStorage;

namespace BlazorEcommerce.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        public event Action OnChange;

        public CartService(ILocalStorageService localStorage, HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            _localStorage = localStorage;
            _http = http;
            _authStateProvider = authStateProvider;
        }

        public async Task AddToCart(CartItem cartItem)
        {
            var isAuthenticated = await IsUserAuthenticated();

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
            var isAuthenticated = await IsUserAuthenticated();

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
                    var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);

                    var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();

                    var result = cartProducts.Data;

                    return result;
                }
            }
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            var isAuthenticated = await IsUserAuthenticated();

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
            var isAuthenticated = await IsUserAuthenticated();

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

        private async Task<bool> IsUserAuthenticated()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var isAuthenticated = state.User.Identity.IsAuthenticated;
            return isAuthenticated;
        }

        public async Task GetCartItemsCount()
        {
            var isAuthenticated = await IsUserAuthenticated();

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
