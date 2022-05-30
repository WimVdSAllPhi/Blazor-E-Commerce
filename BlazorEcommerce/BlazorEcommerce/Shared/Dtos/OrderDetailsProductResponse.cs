namespace BlazorEcommerce.Shared
{
    public class OrderDetailsProductResponse
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string ProductTypeName { get; set; }
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
