namespace BlazorEcommerce.Shared
{
    public class CartProductResponse
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = string.Empty;

        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;

        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
