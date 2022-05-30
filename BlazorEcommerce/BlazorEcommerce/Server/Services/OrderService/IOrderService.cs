namespace BlazorEcommerce.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder(int userId, OrderType orderType);

        Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders();

        Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetails(int orderId);

        Task<ServiceResponse<List<OrderAdmin>>> GetAllOrders();

        Task<ServiceResponse<List<OrderAdmin>>> PutAsDone(int orderId);
    }
}
