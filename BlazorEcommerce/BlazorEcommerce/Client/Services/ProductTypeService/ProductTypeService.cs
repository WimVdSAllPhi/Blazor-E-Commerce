namespace BlazorEcommerce.Client.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        public event Action OnChange;

        public List<ProductType> ProductTypes { get; set; }

        private readonly HttpClient _http;

        public ProductTypeService(HttpClient http)
        {
            _http = http;
        }

        public async Task GetProductTypes()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/producttype");

            ProductTypes = result.Data;
        }

        public async Task AddProductType(ProductType productType)
        {
            var response = await _http.PostAsJsonAsync("api/producttype", productType);

            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>();

            ProductTypes = result.Data;

            OnChange.Invoke();
        }

        public async Task UpdateProductType(ProductType productType)
        {
            var response = await _http.PutAsJsonAsync("api/producttype", productType);

            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>();

            ProductTypes = result.Data;

            OnChange.Invoke();
        }

        public async Task DeleteProductType(int productTypeId)
        {
            var response = await _http.DeleteAsync($"api/ProductType/{productTypeId}");

            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>();

            ProductTypes = result.Data;

            await GetProductTypes();

            OnChange.Invoke();
        }

        public ProductType CreateNewProductType()
        {
            var newProductType = new ProductType { IsNew = true, Editing = true };

            ProductTypes.Add(newProductType);

            OnChange.Invoke();

            return newProductType;
        }
    }
}
