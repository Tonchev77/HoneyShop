namespace HoneyShop.Data
{
    using HoneyShop.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class HoneyShopDbContext : IdentityDbContext
    {
        public HoneyShopDbContext(DbContextOptions<HoneyShopDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Cart> Cart { get; set; } = null!;
        public virtual DbSet<CartItem> CartsItems { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrdersItems { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductPromotion> ProductsPromotions { get; set; } = null!;
        public virtual DbSet<ProductStock> ProductsStocks { get; set; } = null!;
        public virtual DbSet<Promotion> Promotions { get; set; } = null!;
        public virtual DbSet<Warehouse> Warehouses { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
