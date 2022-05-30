namespace BlazorEcommerce.Shared
{
    public class SendMailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string DislplayName { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public string SendGridApiKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderEmailFriendlyName { get; set; }
    }
}
