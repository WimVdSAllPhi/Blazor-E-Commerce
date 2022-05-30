namespace BlazorEcommerce.Server.Data
{
    public class DataContext : DbContext
    {
        // Entities
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }

        public DbSet<Ad> Ads { get; set; }

        // Constructor
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        // Seeding
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>().HasKey(x => new { x.UserId, x.ProductId, x.ProductTypeId }); // Composite PK

            modelBuilder.Entity<ProductVariant>().HasKey(x => new { x.ProductId, x.ProductTypeId }); // Composite PK

            modelBuilder.Entity<OrderItem>().HasKey(x => new { x.OrderId, x.ProductId, x.ProductTypeId }); // Composite PK

            modelBuilder.SeedData();
        }
    }
}
