namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> entity)
        {
            entity
                .HasKey(ci => new { ci.ProductId, ci.CartId });

            entity.HasOne(ci => ci.Cart)
               .WithMany(c => c.Items)
               .HasForeignKey(ci => ci.CartId)
               .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ci => ci.Product)
                  .WithMany()
                  .HasForeignKey(ci => ci.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity
                .Property(ci => ci.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(ci => ci.IsDeleted == false);
        }
    }
}
