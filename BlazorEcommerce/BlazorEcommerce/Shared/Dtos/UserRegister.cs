using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared
{
    public class UserRegister
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0][1-9]([.][0-9][0-9]){4}", ErrorMessage = "Incorrect phone number !")]
        public string PhoneNumber { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
