namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            entity
                .HasKey(oi => new { oi.OrderId, oi.ProductId });

            entity.HasOne(oi => oi.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(oi => oi.Product)
                  .WithMany(p => p.OrderItems)
                  .HasForeignKey(oi => oi.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            entity
                .Property(oi => oi.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(oi => oi.IsDeleted == false);
        }
    }
}
