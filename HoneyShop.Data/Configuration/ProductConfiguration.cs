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

            entity
                .HasData(this.GenerateSeedProduct());
        }

        private List<Product> GenerateSeedProduct() 
        {
            List<Product> productSeed = new List<Product>()
            {
                new Product
                {
                    Id = new Guid("c8a7f282-84b9-41d1-b99a-8e4ea6714e4e"),
                    Name = "Raw Forest Honey",
                    Description = "Pure, unprocessed honey from forest hives. Rich in antioxidants and flavor.",
                    Price = 14.99m,
                    CategoryId = new Guid("6b74e49c-8bfb-4c3d-b91e-3d5b441e9d13"),
                    ImageUrl = "https://www.queenandhoney.com.au/wp-content/uploads/2020/08/HONEY_03.png",
                    CreatorId = "15167365-502c-42be-9f14-3e623c2e465e",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                },
                new Product
                {
                    Id = new Guid("65dcd066-b208-472e-b52c-6a86fbb3545e"),
                    Name = "Propolis Tincture",
                    Description = "Alcohol-based extract of propolis. Supports immune system health.",
                    Price = 9.50m,
                    CategoryId = new Guid("1fbc3a2e-234a-4f9c-a6d8-f55a388ba5a7"),
                    ImageUrl = "https://m.media-amazon.com/images/I/61VdnnN0eOL._UF1000,1000_QL80_.jpg",
                    CreatorId = "15167365-502c-42be-9f14-3e623c2e465e",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                },
                new Product
                {
                    Id = new Guid("db52dc2f-50b7-4fc0-b27f-cb98e88ccff1"),
                    Name = "Beeswax Block",
                    Description = "100% natural beeswax, perfect for DIY cosmetics, candles, and wood polish.",
                    Price = 5.75m,
                    CategoryId = new Guid("9a3ef5c7-7db9-4f4c-98c2-88c772cf8e91"),
                    ImageUrl = "https://images.squarespace-cdn.com/content/v1/58a39f8cff7c503db48b3c43/1643666787081-F6AHQE44NO8QHAKIFYY1/Untitled+design+%282%29.png?format=1000w",
                    CreatorId = "15167365-502c-42be-9f14-3e623c2e465e",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                },
                new Product
                {
                    Id = new Guid("49b5d0ce-37d6-4563-8492-3cf358c1ffb1"),
                    Name = "Bee Pollen Granules",
                    Description = "Dried bee pollen granules. High in vitamins and minerals, ideal as a food supplement.",
                    Price = 12.00m,
                    CategoryId = new Guid("a4f0a2bc-b3de-45ac-a62b-5d0100329a6c"),
                    ImageUrl = "https://www.aratakihoney.co.nz/cdn/shop/files/BeePollenGranulesFront_3069x.png?v=1707957229",
                    CreatorId = "15167365-502c-42be-9f14-3e623c2e465e",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                },
                new Product
                {
                    Id = new Guid("8c4d30cc-faa0-4d60-8c41-f055a6df1d91"),
                    Name = "Royal Jelly Capsules",
                    Description = "Capsules filled with freeze-dried royal jelly. Known for vitality and skin health benefits.",
                    Price = 19.99m,
                    CategoryId = new Guid("c70e8376-0ed9-4265-94e3-b9e80b7cf42e"),
                    ImageUrl = "https://m.media-amazon.com/images/I/71KUhcxVe6L.jpg",
                    CreatorId = "15167365-502c-42be-9f14-3e623c2e465e",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                }
            };

            return productSeed;
        }
    }
}
