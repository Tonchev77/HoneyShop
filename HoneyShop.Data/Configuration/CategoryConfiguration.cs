namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static GCommon.ValidationConstants.Category;
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity
                .HasKey(c => c.Id);

            entity
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(c => c.Description)
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
