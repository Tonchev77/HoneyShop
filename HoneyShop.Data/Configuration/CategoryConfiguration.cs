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

            entity
                .HasData(this.GenerateSeedCategory());
        }

        private List<Category> GenerateSeedCategory() 
        {
            List<Category> seedCategory = new List<Category>()
            {
                new Category
                {
                    Id = Guid.Parse("6b74e49c-8bfb-4c3d-b91e-3d5b441e9d13"),
                    Name = "Honey",
                    Description = "Different types of raw and processed honey.",
                    IsDeleted = false
                },
                new Category
                {
                    Id = Guid.Parse("1fbc3a2e-234a-4f9c-a6d8-f55a388ba5a7"),
                    Name = "Propolis",
                    Description = "Propolis in various forms – tinctures, sprays, and raw.",
                    IsDeleted = false
                },
                new Category
                {
                    Id = Guid.Parse("9a3ef5c7-7db9-4f4c-98c2-88c772cf8e91"),
                    Name = "Beeswax",
                    Description = "Natural beeswax blocks and pellets for crafting or cosmetics.",
                    IsDeleted = false
                },
                new Category
                {
                    Id = Guid.Parse("a4f0a2bc-b3de-45ac-a62b-5d0100329a6c"),
                    Name = "Bee Pollen",
                    Description = "Granules and capsules made from bee pollen.",
                    IsDeleted = false
                },
                new Category
                {
                    Id = Guid.Parse("c70e8376-0ed9-4265-94e3-b9e80b7cf42e"),
                    Name = "Royal Jelly",
                    Description = "Royal jelly in capsules, fresh, or freeze-dried form.",
                    IsDeleted = false
                }
            };

            return seedCategory;
        }
    }
}
