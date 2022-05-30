using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEcommerce.Shared
{
    public class OrderAdmin
    {
        public int Id { get; set; }

        public User? User { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public OrderType OrderType { get; set; }

        public bool IsDone { get; set; } = false;

        public List<OrderItem> OrderItems { get; set; }
    }
}
