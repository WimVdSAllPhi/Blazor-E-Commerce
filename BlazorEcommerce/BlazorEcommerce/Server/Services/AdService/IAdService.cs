
namespace BlazorEcommerce.Server.Services.AdService
{
    public interface IAdService
    {
        Task<ServiceResponse<Ad>> CreateAdAsync(Ad ad);
        Task<ServiceResponse<bool>> DeleteAdAsync(int adId);
        Task<ServiceResponse<List<Ad>>> GetAdsAsync();
    }
}