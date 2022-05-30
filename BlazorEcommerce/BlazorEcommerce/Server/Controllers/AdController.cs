using BlazorEcommerce.Server.Services.AdService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private readonly IAdService _adService;

        public AdController(IAdService adService)
        {
            _adService = adService;
        }

        public async Task<ActionResult<ServiceResponse<List<Ad>>>> GetAdsAsync()
        {
            var result = await _adService.GetAdsAsync();

            return Ok(result);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Ad>>> CreateAdAsync(Ad ad)
        {
            var result = await _adService.CreateAdAsync(ad);
            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteAdAsync(int id)
        {
            var result = await _adService.DeleteAdAsync(id);
            return Ok(result);
        }
    }
}
