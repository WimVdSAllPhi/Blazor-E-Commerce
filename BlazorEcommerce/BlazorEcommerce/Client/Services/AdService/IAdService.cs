namespace BlazorEcommerce.Client.Services.AdService
{
    public interface IAdService
    {
        List<Ad> Ads { get; set; }

        event Action AdsChanged;

        Task<Ad> CreateAd(Ad ad);

        Task DeleteAdAsync(Ad ad);

        Task GetAdsAsync();
    }
}
