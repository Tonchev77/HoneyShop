namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static GCommon.ValidationConstants.OrderStatus;
    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> entity)
        {
            entity
                .HasKey(os => os.Id);

            entity
                .Property(os => os.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(os => os.Description)
                .IsRequired(false)
                .HasMaxLength(DescriptionMaxLength);

            entity
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(p => p.IsDeleted == false);
        }
    }
}
