using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        #region Admin

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProductsAsync()
        {
            var result = await _productService.GetAdminProductsAsync();

            return Ok(result);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProductAsync(Product product)
        {
            var result = await _productService.CreateProductAsync(product);
            return Ok(result);
        }

        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> UpdateProductAsync(Product product)
        {
            var result = await _productService.UpdateProductAsync(product);
            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProductAsync(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            return Ok(result);
        }

        #endregion Admin

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

        [HttpGet("search/{searchText}/{page:int}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResult>>> SearchProductsAsync(string searchText, int page)
        {
            var result = await _productService.SearchProductsAsync(searchText, page);

            return Ok(result);
        }

        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> GetProductSearchSuggestionsAsync(string searchText)
        {
            var result = await _productService.GetProductSearchSuggestionsAsync(searchText);

            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProductsAsync()
        {
            var result = await _productService.GetFeaturedProductsAsync();

            return Ok(result);
        }
    }
}
