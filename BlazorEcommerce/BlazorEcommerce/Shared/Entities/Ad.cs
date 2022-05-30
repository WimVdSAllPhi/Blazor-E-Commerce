using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared
{
    public class Ad
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Url { get; set; }

        public string CssClass { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
