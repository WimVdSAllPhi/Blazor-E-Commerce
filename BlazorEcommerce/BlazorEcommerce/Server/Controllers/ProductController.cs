﻿using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAllProductsAsync()
        {
            var result = await _productService.GetAllProductsAsync();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProductByIdAsync(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategoryAsync(string categoryUrl)
        {
            var result = await _productService.GetProductsByCategoryAsync(categoryUrl);

            return Ok(result);
        }
    }
}
