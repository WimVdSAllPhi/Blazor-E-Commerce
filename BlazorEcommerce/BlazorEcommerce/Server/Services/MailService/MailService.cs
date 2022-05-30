using System.Diagnostics;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BlazorEcommerce.Server.Services.MailService
{
    public class MailService : IMailService
    {
        private readonly SendMailSettings _mailConfig;

        public MailService(SendMailSettings mailConfig)
        {
            _mailConfig = mailConfig;
        }

        public async Task<ServiceResponse<string>> SendEmailAsync(SendMail sendMail)
        {
            var emailMessage = CreateEmailMessage(sendMail);

            var response = await SendAsync(emailMessage);

            return response;
        }

        private SendGridMessage CreateEmailMessage(SendMail sendMail)
        {
            var from = new EmailAddress(_mailConfig.FromEmail, _mailConfig.DislplayName);
            var to = new EmailAddress(sendMail.ToEmail, sendMail.ToEmail);
            var plaintTextContent = sendMail.HTMLBody;
            var htmlContent = sendMail.HTMLBody;
            var subject = sendMail.Subject;

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plaintTextContent, htmlContent);

            return msg;
        }

        private async Task<ServiceResponse<string>> SendAsync(SendGridMessage mailMessage)
        {
            var response = new ServiceResponse<string>();

            var client = new SendGridClient(_mailConfig.SendGridApiKey);

            try
            {
                var result = await client.SendEmailAsync(mailMessage).ConfigureAwait(false);

                if (result != null)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        response.Data = "Mail Sent";
                    }
                    else
                    {
                        var bodyResult = await result.Body.ReadAsStringAsync();

                        var sendGridResponse = JsonConvert.DeserializeObject<SendGridResponse>(bodyResult);

                        var errorMessage = string.Empty;

                        if (sendGridResponse != null && sendGridResponse.Errors != null && sendGridResponse.Errors.Count > 0)
                        {
                            if (sendGridResponse.Errors.Count == 1)
                            {
                                errorMessage = sendGridResponse.Errors[0].Message;
                            }
                            else
                            {
                                for (int i = 0; i < sendGridResponse.Errors.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        errorMessage = sendGridResponse.Errors[i].Message;
                                    }
                                    else
                                    {
                                        errorMessage += $"; {sendGridResponse.Errors[i].Message}";
                                    }
                                }
                            }
                        }
                        else
                        {
                            errorMessage = "Unknow error occured from sending service! Retry or take direct contact.";
                        }

                        response.Success = false;
                        response.Message = errorMessage;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "No Response";
                }
            }
            catch (Exception ex)
            {
                // Break in debug Mode
                if (Debugger.IsAttached)
                    Debugger.Break();

                response.Success = false;
                response.Message = "Unknow error occured!";
            }

            return response;
        }

        public async Task<SendMail> GetBaseMail(SendMail sendMail, string title, string textTitle)
        {
            var templateText = string.Empty;

            // Read the base template from file
            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("BlazorEcommerce.Server.Templates.Base.html"), Encoding.UTF8))
            {
                // Read file contents
                templateText = await reader.ReadToEndAsync();
            }

            // Replace special values with those inside the template
            templateText = templateText.Replace("--Title--", title)
                                        .Replace("--TextTitle--", textTitle)
                                        .Replace("--TextBody--", sendMail.HTMLBody);

            sendMail.HTMLBody = templateText;

            return sendMail;
        }
    }
}
