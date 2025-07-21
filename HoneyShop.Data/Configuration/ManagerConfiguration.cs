namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> entity)
        {
            entity
                .HasKey(m => m.Id);

            entity
                .Property(m => m.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasIndex(m => new { m.UserId })
                .IsUnique();

            entity
                .HasQueryFilter(m => m.IsDeleted == false);
        }
    }
}
