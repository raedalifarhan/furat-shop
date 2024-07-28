using API.Models;
using API.Models.OrderAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product
            modelBuilder.Entity<Product>()
                .Property(p => p.RegularPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.SellingPrice)
                .HasPrecision(18, 2);

            // Order
            modelBuilder.Entity<Order>()
                .Property(p => p.Subtotal)
                .HasPrecision(18, 2);
            
            // Order Item
            modelBuilder.Entity<OrderItem>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
            
            // Basket Item
            modelBuilder.Entity<BasketItem>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
            
            // Delivery Method
            modelBuilder.Entity<DeliveryMethod>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
            
            // Invoice
            modelBuilder.Entity<Invoice>()
                .Property(p => p.ShippingFee)
                .HasPrecision(18, 2);
            
            modelBuilder.Entity<Invoice>()
                .Property(p => p.Taxes)
                .HasPrecision(18, 2);
            
            modelBuilder.Entity<Invoice>()
                .Property(p => p.TotalAmount)
                .HasPrecision(18, 2);

            // Category
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();

            // AppUser
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.CustomerBaskets)
                .WithOne(cb => cb.Customer)
                .HasForeignKey(cb => cb.CustomerId);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CustomerBasket> CustomerBaskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
    }
}