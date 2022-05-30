namespace BlazorEcommerce.Client.Services.AdService
{
    public class AdService : IAdService
    {
        public List<Ad> Ads { get; set; }

        public event Action AdsChanged;

        private readonly HttpClient _http;

        public AdService(HttpClient http)
        {
            _http = http;
        }

        public async Task GetAdsAsync()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Ad>>>("api/Ad");

            if (result != null && result.Data != null)
            {
                Ads = result.Data;
            }

            AdsChanged.Invoke();
        }

        public async Task<Ad> CreateAd(Ad ad)
        {
            var result = await _http.PostAsJsonAsync("api/ad", ad);

            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<Ad>>();

            return content.Data;
        }

        public async Task DeleteAdAsync(Ad ad)
        {
            var result = await _http.DeleteAsync($"api/ad/{ad.Id}");
        }
    }
}
