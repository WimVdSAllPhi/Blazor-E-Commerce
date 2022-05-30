namespace BlazorEcommerce.Client.Services.OrderService
{
    public interface IOrderService
    {
        event Action OnChange;

        List<OrderAdmin> AdminOrders { get; set; }

        Task<string> PlaceOrder(OrderType orderType);

        Task<List<OrderOverviewResponse>> GetOrders();

        Task<OrderDetailsResponse> GetOrderDetails(int orderId);

        Task GetAllOrders();

        Task PutAsDone(int orderId);
    }
}
