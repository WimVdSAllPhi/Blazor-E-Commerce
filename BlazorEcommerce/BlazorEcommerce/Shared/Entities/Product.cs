using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEcommerce.Shared
{
    public class Product
    {
        public int Id { get; set; } // PK

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool Featured { get; set; } = false;

        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;

        // Not add in Db
        [NotMapped]
        public bool Editing { get; set; } = false;

        [NotMapped]
        public bool IsNew { get; set; } = false;

        // FK
        public Category? Category { get; set; }

        public int CategoryId { get; set; }

        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }
}
