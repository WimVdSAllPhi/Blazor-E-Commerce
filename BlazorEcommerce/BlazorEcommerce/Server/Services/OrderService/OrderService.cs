namespace BlazorEcommerce.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public OrderService(DataContext context, ICartService cartService, IAuthService authService)
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetails(int orderId)
        {
            var userId = _authService.GetUserId();

            var response = new ServiceResponse<OrderDetailsResponse>();

            var order = await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.ProductType)
                .Where(x => x.UserId == userId && x.Id == orderId)
                .OrderByDescending(x => x.OrderDate)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                response.Success = false;
                response.Message = "Order not found.";

                return response;
            }

            var orderDetailsResponse = new OrderDetailsResponse
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductResponse>()
            };

            order.OrderItems.ForEach(item =>
            orderDetailsResponse.Products.Add(new OrderDetailsProductResponse
            {
                ProductId = item.ProductId,
                ImageUrl = item.Product.ImageUrl,
                ProductTypeName = item.ProductType.Name,
                Quantity = item.Quantity,
                Title = item.Product.Title,
                TotalPrice = item.TotalPrice
            }));

            response.Data = orderDetailsResponse;

            return response;
        }

        public async Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders()
        {
            var userId = _authService.GetUserId();

            var response = new ServiceResponse<List<OrderOverviewResponse>>();

            var orders = await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();

            var orderResponse = new List<OrderOverviewResponse>();

            orders.ForEach(x => orderResponse.Add(new OrderOverviewResponse
            {
                Id = x.Id,
                OrderDate = x.OrderDate,
                TotalPrice = x.TotalPrice,
                ProductName = x.OrderItems.Count > 1 ?
                    $"{x.OrderItems.First().Product.Title} and {x.OrderItems.Count - 1} more..." :
                    x.OrderItems.First().Product.Title,
                ProductImageUrl = x.OrderItems.First().Product.ImageUrl,
            }));

            response.Data = orderResponse;

            return response;
        }

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            var userId = _authService.GetUserId();

            var productsServiceResponse = await _cartService.GetDbCartProducts();

            var products = productsServiceResponse.Data;

            var totalPrice = 0m;

            products.ForEach(product => totalPrice += product.Price * product.Quantity);

            var orderItems = new List<OrderItem>();

            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity,
            }));

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);

            var cartItemsOfUser = _context.CartItems.Where(x => x.UserId == userId);

            _context.CartItems.RemoveRange(cartItemsOfUser);

            await _context.SaveChangesAsync();

            var response = new ServiceResponse<bool>()
            {
                Data = true,
            };

            return response;
        }
    }
}
