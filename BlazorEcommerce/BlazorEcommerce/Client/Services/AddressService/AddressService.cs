namespace BlazorEcommerce.Client.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient _http;

        public AddressService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Address> AddOrUpdateAddress(Address address)
        {
            var result = await _http.PostAsJsonAsync("api/address", address);

            var content = result.Content.ReadFromJsonAsync<ServiceResponse<Address>>().Result;

            return content.Data;
        }

        public async Task<Address> GetAddress()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Address>>("api/address");

            return result.Data;
        }
    }
}
