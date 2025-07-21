namespace HoneyShop.Data.Configuration
{
    using HoneyShop.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class IdentityUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity
                .HasData(this.CreateDefaultAdminUser());
        }

        private ApplicationUser CreateDefaultAdminUser()
        {
            ApplicationUser defaultUser = new ApplicationUser
            {
                Id = "15167365-502c-42be-9f14-3e623c2e465e",
                UserName = "admin@honeyshop.com",
                NormalizedUserName = "ADMIN@HONEYSHOP.COM",
                Email = "admin@honeyshop.com",
                NormalizedEmail = "ADMIN@HONEYSHOP.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(
                        new ApplicationUser { UserName = "admin@honeyshop.com" },
                        "Admin123!")
            };

            return defaultUser;
        }
    }
}
