namespace BlazorEcommerce.Client.Services.MailService
{
    public interface IMailService
    {
        Task<ServiceResponse<string>> SendEmailAsync(SendMail request);
    }
}
