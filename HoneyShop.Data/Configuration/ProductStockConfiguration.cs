namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
    {
        public void Configure(EntityTypeBuilder<ProductStock> entity)
        {
            entity.
                HasKey(ps => new { ps.ProductId, ps.WarehouseId });

            entity
                .HasOne(ps => ps.Warehouse)
                .WithMany(w => w.ProductStocks)
                .HasForeignKey(ps => ps.WarehouseId);

            entity
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductStocks)
                .HasForeignKey(ps => ps.ProductId);

            entity
                .Property(ps => ps.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(ps => ps.IsDeleted == false);
        }
    }
}
