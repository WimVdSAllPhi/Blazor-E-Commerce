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
                .ThenInclude(x => x.ProductImages)
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
                ProductImages = item.Product.ProductImages,
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
                .ThenInclude(x => x.ProductImages)
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
                ProductImageUrl = x.OrderItems.First().Product.ProductImages != null && x.OrderItems.First().Product.ProductImages.Count > 0 ? x.OrderItems.First().Product.ProductImages[0].ImageUrl : string.Empty,
                OrderType = x.OrderType,
                IsDone = x.IsDone,
            }));

            response.Data = orderResponse;

            return response;
        }

        public async Task<ServiceResponse<bool>> PlaceOrder(int userId, OrderType orderType)
        {
            var productsServiceResponse = await _cartService.GetDbCartProducts(userId);

            var products = productsServiceResponse.Data;

            var totalPrice = 0m;

            if (products != null && products.Count > 0)
            {
                foreach (var product in products)
                {
                    var dbProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == product.ProductId);

                    if (dbProduct != null)
                    {
                        dbProduct.Variants.First(x => x.ProductTypeId == product.ProductTypeId).Stock -= product.Quantity;
                    }
                }
            }

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
                OrderItems = orderItems,
                OrderType = orderType
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

        public async Task<ServiceResponse<List<OrderAdmin>>> GetAllOrders()
        {
            var userId = _authService.GetUserId();

            var response = new ServiceResponse<List<OrderAdmin>>();

            var orders = await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.ProductType)
                .Include(x => x.User)
                .ThenInclude(x => x.Address)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();

            var orderResponse = new List<OrderAdmin>();

            orders.ForEach(x => orderResponse.Add(new OrderAdmin
            {
                Id = x.Id,
                OrderDate = x.OrderDate,
                TotalPrice = x.TotalPrice,
                OrderType = x.OrderType,
                IsDone = x.IsDone,
                OrderItems = x.OrderItems.Select(x => new OrderItem
                {
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    Product = x.Product,
                    ProductType = x.ProductType,
                    ProductTypeId = x.ProductTypeId,
                    Quantity = x.Quantity,
                    TotalPrice = x.TotalPrice,
                }).ToList(),
                User = x.User,
                UserId = userId
            }));

            response.Data = orderResponse;

            return response;
        }

        public async Task<ServiceResponse<List<OrderAdmin>>> PutAsDone(int orderId)
        {
            var dbOrder = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

            if (dbOrder == null)
            {
                var errorResponse = new ServiceResponse<List<OrderAdmin>>()
                {
                    Success = false,
                    Message = "Order does not exist."
                };

                return errorResponse;
            }

            dbOrder.IsDone = true;

            await _context.SaveChangesAsync();

            var orderAdmin = await GetAllOrders();

            return orderAdmin;
        }
    }
}
