namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static GCommon.ValidationConstants.Promotion;
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> entity)
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(NameMaxLength);

            entity.Property(p => p.DiscountPercentage)
                  .IsRequired();

            entity
                .Property(p => p.Description)
                .HasMaxLength(DescriptionMaxLength);

            entity
                .Property(p => p.DiscountPercentage)
                .HasPrecision(5, 2);

            entity
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(p => p.IsDeleted == false);
        }
    }
}
