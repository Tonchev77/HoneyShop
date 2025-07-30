#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HoneyShop.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class InitialSeedForStatusesAndWarehouses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "15167365-502c-42be-9f14-3e623c2e465e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0dd0d4e8-69a2-4a65-a04c-cab80edf77c2", "AQAAAAIAAYagAAAAEIHpRDO+tPNlRCs80OV46T8wdKq+HvVHBMBxwh8k2BC+5pZNPTKb3RESR0rqe89qKA==", "e0720c8b-d544-484b-a754-9f2c0f83e0df" });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "Id", "DateleteAt", "Description", "ManagerId", "Name" },
                values: new object[,]
                {
                    { new Guid("06b0c2ce-cb3c-4226-8141-12abf6f6c349"), null, "Products sent to client", null, "Sent" },
                    { new Guid("368c3173-5aed-49b5-bcff-e61a72c4f0bb"), null, "Confirmed order", null, "Confirmed" },
                    { new Guid("c50fadf4-3045-45f9-beae-c7ff9ff63168"), null, "Pending order/New oreders", null, "Pending" },
                    { new Guid("ce534e02-6cf8-48a3-8abf-75597247fdce"), null, "Products received by client", null, "Finished" }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("49b5d0ce-37d6-4563-8492-3cf358c1ffb1"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 21, 25, 27, 29, DateTimeKind.Utc).AddTicks(8881));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("65dcd066-b208-472e-b52c-6a86fbb3545e"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 21, 25, 27, 29, DateTimeKind.Utc).AddTicks(8872));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8c4d30cc-faa0-4d60-8c41-f055a6df1d91"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 21, 25, 27, 29, DateTimeKind.Utc).AddTicks(8884));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c8a7f282-84b9-41d1-b99a-8e4ea6714e4e"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 21, 25, 27, 29, DateTimeKind.Utc).AddTicks(8866));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("db52dc2f-50b7-4fc0-b27f-cb98e88ccff1"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 21, 25, 27, 29, DateTimeKind.Utc).AddTicks(8876));

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "DeletedAt", "Location", "ManagerId", "Name" },
                values: new object[,]
                {
                    { new Guid("2ab2cb22-b792-4820-d9a9-08ddcdf4d97d"), null, "Ruse, str. Studentska 10", null, "Ruse Warehouse" },
                    { new Guid("97768f5b-38c2-40c4-d9aa-08ddcdf4d97d"), null, "Popovo, str. Mladost 116", null, "Popovo Warehouse" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "15167365-502c-42be-9f14-3e623c2e465e");

            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "Id",
                keyValue: new Guid("06b0c2ce-cb3c-4226-8141-12abf6f6c349"));

            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "Id",
                keyValue: new Guid("368c3173-5aed-49b5-bcff-e61a72c4f0bb"));

            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "Id",
                keyValue: new Guid("c50fadf4-3045-45f9-beae-c7ff9ff63168"));

            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "Id",
                keyValue: new Guid("ce534e02-6cf8-48a3-8abf-75597247fdce"));

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("2ab2cb22-b792-4820-d9a9-08ddcdf4d97d"));

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("97768f5b-38c2-40c4-d9aa-08ddcdf4d97d"));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "15167365-502c-42be-9f14-3e623c2e465e", 0, "310476f2-3baa-48c5-b1c3-92bed8c2539f", "IdentityUser", "admin@honeyshop.com", true, false, null, "ADMIN@HONEYSHOP.COM", "ADMIN@HONEYSHOP.COM", "AQAAAAIAAYagAAAAEFgVIyjZ2y13obb2XQj4dZb0ASMI0b/YAX+JVxGyp0GV6tiaGvRrcCXPUu9WLKiplg==", null, false, "48fcb24f-cdc4-4a8b-ac62-e1174ba69232", false, "admin@honeyshop.com" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("49b5d0ce-37d6-4563-8492-3cf358c1ffb1"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 22, 32, 0, 873, DateTimeKind.Utc).AddTicks(9894));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("65dcd066-b208-472e-b52c-6a86fbb3545e"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 22, 32, 0, 873, DateTimeKind.Utc).AddTicks(9887));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8c4d30cc-faa0-4d60-8c41-f055a6df1d91"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 22, 32, 0, 873, DateTimeKind.Utc).AddTicks(9898));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c8a7f282-84b9-41d1-b99a-8e4ea6714e4e"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 22, 32, 0, 873, DateTimeKind.Utc).AddTicks(9881));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("db52dc2f-50b7-4fc0-b27f-cb98e88ccff1"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 22, 32, 0, 873, DateTimeKind.Utc).AddTicks(9891));
        }
    }
}
