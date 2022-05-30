namespace BlazorEcommerce.Server.Services.PaypalService
{
    public interface IPaypalService
    {
        Task<string> MakePaymentPaypalAsync();
    }
}
