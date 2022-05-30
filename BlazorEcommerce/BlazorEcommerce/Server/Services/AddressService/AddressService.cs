namespace BlazorEcommerce.Server.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;

        public AddressService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<ServiceResponse<Address>> AddOrUpdateAddressAsync(Address address)
        {
            var userId = _authService.GetUserId();

            var response = new ServiceResponse<Address>();

            var dbAddress = (await GetAddress()).Data;

            if (dbAddress == null)
            {
                address.UserId = userId;

                _context.Addresses.Add(address);

                response.Data = address;
            }
            else
            {
                dbAddress.Country = address.Country;
                dbAddress.City = address.City;
                dbAddress.Zip = address.Zip;
                dbAddress.Street = address.Street;
                dbAddress.StreetNr = address.StreetNr;

                response.Data = dbAddress;
            }

            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<ServiceResponse<Address>> GetAddress()
        {
            var userId = _authService.GetUserId();

            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.UserId == userId);

            var response = new ServiceResponse<Address>
            {
                Data = address
            };

            return response;
        }
    }
}
