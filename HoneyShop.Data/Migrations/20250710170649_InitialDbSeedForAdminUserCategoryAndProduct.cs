#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HoneyShop.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class InitialDbSeedForAdminUserCategoryAndProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "15167365-502c-42be-9f14-3e623c2e465e", 0, "404468ad-679f-4d60-b807-748862dab028", "admin@honeyshop.com", true, false, null, "ADMIN@HONEYSHOP.COM", "ADMIN@HONEYSHOP.COM", "AQAAAAIAAYagAAAAEKQC4MvE4VhHK6/GjbRof29eehVsOWuoexbYiUs722xLjU+xO/dKeofgEMODoN8nYA==", null, false, "f4222559-0e8e-4efa-9ed3-6a313c82916c", false, "admin@honeyshop.com" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DateleteAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("1fbc3a2e-234a-4f9c-a6d8-f55a388ba5a7"), null, "Propolis in various forms – tinctures, sprays, and raw.", "Propolis" },
                    { new Guid("6b74e49c-8bfb-4c3d-b91e-3d5b441e9d13"), null, "Different types of raw and processed honey.", "Honey" },
                    { new Guid("9a3ef5c7-7db9-4f4c-98c2-88c772cf8e91"), null, "Natural beeswax blocks and pellets for crafting or cosmetics.", "Beeswax" },
                    { new Guid("a4f0a2bc-b3de-45ac-a62b-5d0100329a6c"), null, "Granules and capsules made from bee pollen.", "Bee Pollen" },
                    { new Guid("c70e8376-0ed9-4265-94e3-b9e80b7cf42e"), null, "Royal jelly in capsules, fresh, or freeze-dried form.", "Royal Jelly" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "CreatorId", "DeletedAt", "Description", "ImageUrl", "IsActive", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("49b5d0ce-37d6-4563-8492-3cf358c1ffb1"), new Guid("a4f0a2bc-b3de-45ac-a62b-5d0100329a6c"), new DateTime(2025, 7, 10, 17, 6, 48, 526, DateTimeKind.Utc).AddTicks(6431), "15167365-502c-42be-9f14-3e623c2e465e", null, "Dried bee pollen granules. High in vitamins and minerals, ideal as a food supplement.", "https://www.aratakihoney.co.nz/cdn/shop/files/BeePollenGranulesFront_3069x.png?v=1707957229", true, "Bee Pollen Granules", 12.00m },
                    { new Guid("65dcd066-b208-472e-b52c-6a86fbb3545e"), new Guid("1fbc3a2e-234a-4f9c-a6d8-f55a388ba5a7"), new DateTime(2025, 7, 10, 17, 6, 48, 526, DateTimeKind.Utc).AddTicks(6422), "15167365-502c-42be-9f14-3e623c2e465e", null, "Alcohol-based extract of propolis. Supports immune system health.", "https://m.media-amazon.com/images/I/61VdnnN0eOL._UF1000,1000_QL80_.jpg", true, "Propolis Tincture", 9.50m },
                    { new Guid("8c4d30cc-faa0-4d60-8c41-f055a6df1d91"), new Guid("c70e8376-0ed9-4265-94e3-b9e80b7cf42e"), new DateTime(2025, 7, 10, 17, 6, 48, 526, DateTimeKind.Utc).AddTicks(6434), "15167365-502c-42be-9f14-3e623c2e465e", null, "Capsules filled with freeze-dried royal jelly. Known for vitality and skin health benefits.", "https://m.media-amazon.com/images/I/71KUhcxVe6L.jpg", true, "Royal Jelly Capsules", 19.99m },
                    { new Guid("c8a7f282-84b9-41d1-b99a-8e4ea6714e4e"), new Guid("6b74e49c-8bfb-4c3d-b91e-3d5b441e9d13"), new DateTime(2025, 7, 10, 17, 6, 48, 526, DateTimeKind.Utc).AddTicks(6417), "15167365-502c-42be-9f14-3e623c2e465e", null, "Pure, unprocessed honey from forest hives. Rich in antioxidants and flavor.", "https://www.queenandhoney.com.au/wp-content/uploads/2020/08/HONEY_03.png", true, "Raw Forest Honey", 14.99m },
                    { new Guid("db52dc2f-50b7-4fc0-b27f-cb98e88ccff1"), new Guid("9a3ef5c7-7db9-4f4c-98c2-88c772cf8e91"), new DateTime(2025, 7, 10, 17, 6, 48, 526, DateTimeKind.Utc).AddTicks(6427), "15167365-502c-42be-9f14-3e623c2e465e", null, "100% natural beeswax, perfect for DIY cosmetics, candles, and wood polish.", "https://images.squarespace-cdn.com/content/v1/58a39f8cff7c503db48b3c43/1643666787081-F6AHQE44NO8QHAKIFYY1/Untitled+design+%282%29.png?format=1000w", true, "Beeswax Block", 5.75m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("49b5d0ce-37d6-4563-8492-3cf358c1ffb1"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("65dcd066-b208-472e-b52c-6a86fbb3545e"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8c4d30cc-faa0-4d60-8c41-f055a6df1d91"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c8a7f282-84b9-41d1-b99a-8e4ea6714e4e"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("db52dc2f-50b7-4fc0-b27f-cb98e88ccff1"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "15167365-502c-42be-9f14-3e623c2e465e");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("1fbc3a2e-234a-4f9c-a6d8-f55a388ba5a7"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6b74e49c-8bfb-4c3d-b91e-3d5b441e9d13"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9a3ef5c7-7db9-4f4c-98c2-88c772cf8e91"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a4f0a2bc-b3de-45ac-a62b-5d0100329a6c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c70e8376-0ed9-4265-94e3-b9e80b7cf42e"));
        }
    }
}
