using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared
{
    public class SendMail
    {
        // string ToEmail, string Subject, string HTMLBody
        [Required]
        [EmailAddress]
        public string ToEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string HTMLBody { get; set; }
    }
}
