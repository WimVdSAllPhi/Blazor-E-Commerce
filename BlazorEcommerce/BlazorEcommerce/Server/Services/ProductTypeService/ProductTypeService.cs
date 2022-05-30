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

        public async Task<ServiceResponse<List<ProductType>>> DeleteProductType(int id)
        {
            var productType = await GetProductTypeById(id);

            if (productType == null)
            {
                var errorResponse = new ServiceResponse<List<ProductType>>()
                {
                    Success = false,
                    Message = "Product Type not found."
                };

                return errorResponse;
            }

            _context.ProductTypes.Remove(productType);

            await _context.SaveChangesAsync();

            var productTypes = await GetProductTypes();

            return productTypes;
        }

        private async Task<ProductType> GetProductTypeById(int id)
        {
            var productType = await _context.ProductTypes.FirstOrDefaultAsync(c => c.Id == id);

            return productType;
        }
    }
}
