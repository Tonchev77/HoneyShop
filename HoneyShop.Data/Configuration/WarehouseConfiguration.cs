namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static GCommon.ValidationConstants.Warehouse;
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> entity)
        {
            entity
                .HasKey(wh => wh.Id);

            entity
                .Property(wh => wh.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(wh => wh.Location)
                .IsRequired()
                .HasMaxLength(LocationMaxLength);

            entity
               .Property(wh => wh.IsDeleted)
               .HasDefaultValue(false);

            entity
                .HasQueryFilter(wh => wh.IsDeleted == false);
        }
    }
}
