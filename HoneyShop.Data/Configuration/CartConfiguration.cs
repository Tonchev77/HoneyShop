namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> entity)
        {
            entity.HasKey(c => c.Id);

            entity.HasOne(c => c.User)
                  .WithMany()
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.Items)
                  .WithOne(i => i.Cart)
                  .HasForeignKey(i => i.CartId);

            entity
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(c => c.IsDeleted == false);

        }
    }
}
