namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static GCommon.ValidationConstants.Order;
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity
                .HasKey(o => o.Id);

            entity
                .Property(o => o.ShippingCity)
                .IsRequired()
                .HasMaxLength(ShippingCityMaxLength);

            entity
                .Property(o => o.ShippingAddress)
                .IsRequired()
                .HasMaxLength(ShippingAddressMaxLength);

            entity
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            entity
                .Property(o => o.UserId)
                .IsRequired();

            entity
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(o => o.OrderStatus)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.OrderStatusId);

            entity
                .Property(o => o.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(o => o.IsDeleted == false);

        }
    }
}
