namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductPromotionConfiguration : IEntityTypeConfiguration<ProductPromotion>
    {
        public void Configure(EntityTypeBuilder<ProductPromotion> entity)
        {
            entity
                .HasKey(pp => new { pp.ProductId, pp.PromotionId });

            entity.HasOne(pp => pp.Product)
                  .WithMany(p => p.ProductPromotions)
                  .HasForeignKey(pp => pp.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(pp => pp.Promotion)
                  .WithMany(p => p.ProductPromotions)
                  .HasForeignKey(pp => pp.PromotionId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity
                .Property(pp => pp.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(pp => pp.IsDeleted == false);
        }
    }
}
