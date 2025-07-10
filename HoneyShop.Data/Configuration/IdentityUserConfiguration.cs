namespace HoneyShop.Data.Configuration
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> entity)
        {
            entity
                .HasData(this.CreateDefaultAdminUser());
        }

        private IdentityUser CreateDefaultAdminUser()
        {
            IdentityUser defaultUser = new IdentityUser
            {
                Id = "15167365-502c-42be-9f14-3e623c2e465e",
                UserName = "admin@honeyshop.com",
                NormalizedUserName = "ADMIN@HONEYSHOP.COM",
                Email = "admin@honeyshop.com",
                NormalizedEmail = "ADMIN@HONEYSHOP.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                        new IdentityUser { UserName = "admin@honeyshop.com" },
                        "Admin123!")
            };

            return defaultUser;
        }
    }
}
