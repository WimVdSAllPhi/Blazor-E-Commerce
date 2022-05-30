using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IAuthService authService, IMailService mailService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _mailService = mailService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
        {
            var user = new User
            {
                Email = request.Email,
                LastName = request.LastName,
                FirstName = request.FirstName,
                ImageUrl = request.ImageUrl,
                PhoneNumber = request.PhoneNumber,
            };

            var response = await _authService.Register(user, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var basePath = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value;

            var sendMail = await _mailService.GetBaseMail(new SendMail() { ToEmail = request.Email, Subject = "Registration", HTMLBody = $"Thank you to registrat to <a href=\"{basePath}\">K&A Dreamdeals</a>" }, "Registration", $"Welkom {user.FirstName} {user.LastName}");

            var mailResponse = await _mailService.SendEmailAsync(sendMail);

            if (!mailResponse.Success)
            {
                var removeResponse = await _authService.RemoveUser(user);

                if (removeResponse.Success)
                {
                    return BadRequest(new ServiceResponse<int>() { Success = false, Message = "The email given is not real!" });
                }

                return BadRequest(removeResponse);
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
        {
            var response = await _authService.Login(request.Email, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("change-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _authService.ChangePassword(int.Parse(userId), newPassword);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ServiceResponse<UserProfile>>> GetProfileAsync(int id)
        {
            var response = await _authService.GetFullUserByIdAsync(id);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<UserProfile>>> UpdateProductAsync(UserProfile user)
        {
            var result = await _authService.UpdateUserAsync(user);
            return Ok(result);
        }
    }
}
