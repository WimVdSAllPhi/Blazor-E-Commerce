using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<string>>> SendMailAsync(SendMail sendMail)
        {
            sendMail = await _mailService.GetBaseMail(sendMail, "Thank you to reaching out to us.", $"{sendMail.ToEmail} asked:");

            var response = await _mailService.SendEmailAsync(sendMail);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
