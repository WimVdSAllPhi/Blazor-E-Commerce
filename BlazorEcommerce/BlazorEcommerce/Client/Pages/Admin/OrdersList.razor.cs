namespace BlazorEcommerce.Client.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public partial class OrdersList : ComponentBase, IDisposable
    {
        #region Inject

        [Inject] private IOrderService _orderService { get; set; }
        [Inject] private IPrintingService _printingService { get; set; }

        #endregion Inject

        #region Private Properties

        private List<OrderAdmin> _orders = null;

        #endregion Private Properties

        #region Override Methods

        protected override async Task OnInitializedAsync()
        {
            await _orderService.GetAllOrders();

            _orderService.OnChange += StateHasChanged;
        }

        #endregion Override Methods

        #region Private Methods

        private async void PutAsDone(int orderId)
        {
            await _orderService.PutAsDone(orderId);
        }

        private string MapPhoneNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return string.Empty;
            }

            if (number.Length == 10)
            {
                return number.Substring(0, 4) + "/" + number.Substring(4, 2) + "." + number.Substring(6, 2) + "." + number.Substring(8);
            }

            return number;
        }

        public void Dispose()
        {
            _orderService.OnChange -= StateHasChanged;
        }

        #endregion Private Methods
    }
}
