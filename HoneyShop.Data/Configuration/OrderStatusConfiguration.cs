namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static GCommon.ValidationConstants.OrderStatus;
    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> entity)
        {
            entity
                .HasKey(os => os.Id);

            entity
                .Property(os => os.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(os => os.Description)
                .IsRequired(false)
                .HasMaxLength(DescriptionMaxLength);

            entity
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(p => p.IsDeleted == false);

            entity
                .HasData(this.GenerateSeedStatuses());
        }

    private List<OrderStatus> GenerateSeedStatuses()
    {
        List<OrderStatus> statusSeed = new List<OrderStatus>()
        {
            new OrderStatus
            {
                Id = new Guid("c50fadf4-3045-45f9-beae-c7ff9ff63168"),
                Name = "Pending",
                    Description = "Pending order/New oreders",
                    IsDeleted = false
                },
                new OrderStatus
                {
                    Id = new Guid("368c3173-5aed-49b5-bcff-e61a72c4f0bb"),
                    Name = "Confirmed",
                    Description = "Confirmed order",
                    IsDeleted = false
                },
                new OrderStatus
                {
                    Id = new Guid("06b0c2ce-cb3c-4226-8141-12abf6f6c349"),
                    Name = "Sent",
                    Description = "Products sent to client",
                    IsDeleted = false
                },
                new OrderStatus
                {
                    Id = new Guid("ce534e02-6cf8-48a3-8abf-75597247fdce"),
                    Name = "Finished",
                    Description = "Products received by client",
                    IsDeleted = false
                },
            };
            return statusSeed;
        }
    }
}
