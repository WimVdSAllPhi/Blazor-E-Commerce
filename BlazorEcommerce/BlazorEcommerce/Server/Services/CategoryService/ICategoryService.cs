﻿namespace BlazorEcommerce.Server.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetAllCategoriesAsync();

        Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync();

        Task<ServiceResponse<List<Category>>> AddCategory(Category category);

        Task<ServiceResponse<List<Category>>> UpdateCategory(Category category);

        Task<ServiceResponse<List<Category>>> DeleteCategory(int id);
    }
}
