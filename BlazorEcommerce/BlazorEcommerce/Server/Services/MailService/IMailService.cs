namespace BlazorEcommerce.Server.Services.MailService
{
    public interface IMailService
    {
        Task<ServiceResponse<string>> SendEmailAsync(SendMail sendMail);

        Task<SendMail> GetBaseMail(SendMail sendMail, string title, string textTitle);
    }
}
