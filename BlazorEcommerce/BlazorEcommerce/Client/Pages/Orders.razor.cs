namespace BlazorEcommerce.Client.Pages
{
    public partial class Orders : ComponentBase
    {
        #region Inject

        [Inject] private IOrderService OrderService { get; set; }

        #endregion Inject

        #region Private Properties

        private List<OrderOverviewResponse> orders = null;

        #endregion Private Properties

        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            orders = await OrderService.GetOrders();
        }

        #endregion Override Methods
    }
}
