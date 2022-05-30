namespace BlazorEcommerce.Shared
{
    public class PaypalAccessToken
    {
        public string Scope { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string AppId { get; set; }
        public string ExpiresIn { get; set; }
        public string Nonce { get; set; }
    }
}
