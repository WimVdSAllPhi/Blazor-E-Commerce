namespace BlazorEcommerce.Client.Pages
{
    public partial class OrderDetails : ComponentBase
    {
        #region Parameter

        [Parameter] public int OrderId { get; set; }

        #endregion Parameter

        #region Inject

        [Inject] private IOrderService _orderService { get; set; }

        #endregion Inject

        #region Private Properties

        private OrderDetailsResponse _order = null;

        #endregion Private Properties

        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            _order = await _orderService.GetOrderDetails(OrderId);
        }

        #endregion Override Methods
    }
}
