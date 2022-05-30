using System.Text.Json.Serialization;

namespace BlazorEcommerce.Shared
{
    public class ProductImage
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }

        public int ProductId { get; set; }
    }
}
