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
            var response = await _http.PostAsJsonAsync("api/address", address);

            var result = response.Content.ReadFromJsonAsync<ServiceResponse<Address>>().Result;

            return result.Data;
        }

        public async Task<Address> GetAddress()
        {
            var resposne = await _http.GetFromJsonAsync<ServiceResponse<Address>>("api/address");

            return resposne.Data;
        }
    }
}
