using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required]
        public string Street { get; set; } = string.Empty;

        [Required]
        public string StreetNr { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Zip { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = "BE";
    }
}
