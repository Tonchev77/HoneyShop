namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static GCommon.ValidationConstants.Product;
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity
                .HasKey(p => p.Id);

            entity
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);

            entity
                .Property(p => p.ImageUrl)
                .IsRequired()
                .HasMaxLength(ImageUrlMaxLength);

            entity
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            entity
                .Property(p => p.CreatorId)
                .IsRequired();

            entity
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(p => p.IsDeleted == false);

            entity
                .Property(p => p.IsActive)
                .HasDefaultValue(true);
            entity
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
