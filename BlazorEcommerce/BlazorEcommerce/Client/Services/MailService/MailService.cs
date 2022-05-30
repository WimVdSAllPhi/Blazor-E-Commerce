namespace BlazorEcommerce.Client.Services.MailService
{
    public class MailService : IMailService
    {
        private readonly HttpClient _http;

        public MailService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<string>> SendEmailAsync(SendMail request)
        {
            var result = await _http.PostAsJsonAsync("api/mail", request);

            var content = result.Content.ReadFromJsonAsync<ServiceResponse<string>>().Result;

            return content;
        }
    }
}
