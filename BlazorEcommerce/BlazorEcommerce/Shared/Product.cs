using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEcommerce.Shared
{
    public class Product
    {
        public int Id { get; set; } // PK
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")] // => needed for db
        public decimal Price { get; set; }

        // FK
        public Category? Category { get; set; }

        public int CategoryId { get; set; }
    }
}
