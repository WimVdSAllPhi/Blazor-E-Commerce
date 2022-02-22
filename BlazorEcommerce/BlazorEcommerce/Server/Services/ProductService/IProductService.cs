﻿namespace BlazorEcommerce.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetAllProductsAsync();

        Task<ServiceResponse<Product>> GetProductByIdAsync(int id);
    }
}
