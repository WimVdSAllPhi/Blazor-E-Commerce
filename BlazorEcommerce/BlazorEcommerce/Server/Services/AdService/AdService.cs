namespace BlazorEcommerce.Server.Services.AdService
{
    public class AdService : IAdService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<List<Ad>>> GetAdsAsync()
        {
            var response = new ServiceResponse<List<Ad>>();

            var list = await _context.Ads
                .ToListAsync();

            response.Data = list;

            return response;
        }

        public async Task<ServiceResponse<Ad>> CreateAdAsync(Ad ad)
        {
            _context.Ads.Add(ad);

            await _context.SaveChangesAsync();

            var response = new ServiceResponse<Ad>
            {
                Data = ad
            };

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteAdAsync(int adId)
        {
            var dbAd = await _context.Ads.FindAsync(adId);

            if (dbAd == null)
            {
                var errorResponse = new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Ad not found."
                };

                return errorResponse;
            }

            _context.Ads.Remove(dbAd);

            await _context.SaveChangesAsync();

            var response = new ServiceResponse<bool>
            {
                Data = true
            };

            return response;
        }
    }
}
