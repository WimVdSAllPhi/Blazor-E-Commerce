namespace BlazorEcommerce.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext _context;

        public ProductTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
        {
            productType.Editing = productType.IsNew = false;

            _context.ProductTypes.Add(productType);

            await _context.SaveChangesAsync();

            var response = await GetProductTypes();

            return response;
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
        {
            var productTypes = await _context.ProductTypes.ToListAsync();

            var response = new ServiceResponse<List<ProductType>> { Data = productTypes };

            return response;
        }

        public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
        {
            var dbProductType = await _context.ProductTypes.FindAsync(productType.Id);

            if (dbProductType == null)
            {
                var errroResponse = new ServiceResponse<List<ProductType>>
                {
                    Success = false,
                    Message = "Product Type not found."
                };

                return errroResponse;
            }

            dbProductType.Name = productType.Name;

            await _context.SaveChangesAsync();

            var response = await GetProductTypes();

            return response;
        }
    }
}
