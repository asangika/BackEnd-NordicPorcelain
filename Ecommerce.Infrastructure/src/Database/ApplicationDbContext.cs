using Ecommerce.Domain.src.AddressAggregate;
using Ecommerce.Domain.src.CategoryAggregate;
using Ecommerce.Domain.src.Entities.OrderAggregate;
using Ecommerce.Domain.src.Entities.PaymentAggregate;
using Ecommerce.Domain.src.Entities.ReviewAggregate;
using Ecommerce.Domain.src.Entities.ShipmentAggregate;
using Ecommerce.Domain.src.Entities.UserAggregate;
using Ecommerce.Domain.src.PaymentAggregate;
using Ecommerce.Domain.src.ProductAggregate;
using Ecommerce.Domain.src.Shared;
using Ecommerce.Domain.src.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.src.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Shipment> Shipments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseEntity>().HasKey(b => b.Id);
            modelBuilder.Entity<BaseEntity>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<BaseEntity>()
                .Property(b => b.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<OrderItem>()
        .HasOne(oi => oi.Product)
        .WithMany(p => p.OrderItems)
        .HasForeignKey(oi => oi.ProductId)
        .OnDelete(DeleteBehavior.Cascade); // or DeleteBehavior.NoAction

            // Configuring the delete behavior for Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // or DeleteBehavior.NoAction

            modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            // Configuring the delete behavior for Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade); // or DeleteBehavior.NoAction
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
            modelBuilder.Entity<Payment>().ToTable("Payments");
            modelBuilder.Entity<PaymentMethod>().ToTable("PaymentMethods");
            modelBuilder.Entity<Review>().ToTable("Reviews");
            modelBuilder.Entity<Shipment>().ToTable("Shipments");
            modelBuilder.Entity<Address>().ToTable("Addresses");
            modelBuilder.Entity<UserAddress>().ToTable("UserAddresses");
            modelBuilder.Entity<ProductImage>().ToTable("ProductImages");
        }


    }
}