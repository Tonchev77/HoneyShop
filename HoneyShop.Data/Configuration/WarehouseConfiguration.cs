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

            entity
                .HasData(this.GenerateSeedWarehouses());
        }
        private List<Warehouse> GenerateSeedWarehouses()
        {
            List<Warehouse> warehouseSeed = new List<Warehouse>()
        {
            new Warehouse
            {
                Id = new Guid("2ab2cb22-b792-4820-d9a9-08ddcdf4d97d"),
                Name = "Ruse Warehouse",
                Location = "Ruse, str. Studentska 10",
                IsDeleted = false
            },
            new Warehouse
            {
                Id = new Guid("97768f5b-38c2-40c4-d9aa-08ddcdf4d97d"),
                Name = "Popovo Warehouse",
                Location = "Popovo, str. Mladost 116",
                IsDeleted = false
                },
            };
            return warehouseSeed;
        }
    }
}
