#nullable disable

namespace HoneyShop.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;
    /// <inheritdoc />
    public partial class AddingApplicationUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "15167365-502c-42be-9f14-3e623c2e465e");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Cart",
                type: "nvarchar(450)",
                nullable: true);

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
                columns: new[] { "ApplicationUserId", "CreatedAt" },
                values: new object[] { null, new DateTime(2025, 7, 21, 22, 32, 0, 873, DateTimeKind.Utc).AddTicks(9894) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("65dcd066-b208-472e-b52c-6a86fbb3545e"),
                columns: new[] { "ApplicationUserId", "CreatedAt" },
                values: new object[] { null, new DateTime(2025, 7, 21, 22, 32, 0, 873, DateTimeKind.Utc).AddTicks(9887) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8c4d30cc-faa0-4d60-8c41-f055a6df1d91"),
                columns: new[] { "ApplicationUserId", "CreatedAt" },
                values: new object[] { null, new DateTime(2025, 7, 21, 22, 32, 0, 873, DateTimeKind.Utc).AddTicks(9898) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c8a7f282-84b9-41d1-b99a-8e4ea6714e4e"),
                columns: new[] { "ApplicationUserId", "CreatedAt" },
                values: new object[] { null, new DateTime(2025, 7, 21, 22, 32, 0, 873, DateTimeKind.Utc).AddTicks(9881) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("db52dc2f-50b7-4fc0-b27f-cb98e88ccff1"),
                columns: new[] { "ApplicationUserId", "CreatedAt" },
                values: new object[] { null, new DateTime(2025, 7, 21, 22, 32, 0, 873, DateTimeKind.Utc).AddTicks(9891) });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ApplicationUserId",
                table: "Products",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ApplicationUserId",
                table: "Cart",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_AspNetUsers_ApplicationUserId",
                table: "Cart",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_AspNetUsers_UserId",
                table: "Managers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_ApplicationUserId",
                table: "Products",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_AspNetUsers_ApplicationUserId",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_AspNetUsers_UserId",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationUserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_ApplicationUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ApplicationUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Cart_ApplicationUserId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "15167365-502c-42be-9f14-3e623c2e465e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2247c65e-6782-44e2-93ee-0d8d6caf771e", "AQAAAAIAAYagAAAAEHjWFnP5ny6LB9Hr4cNgsn0D0yyWhwrGS1BrKcQK1Yrzz/FUa/OQ8J+c0xD8lV+FIg==", "cd750359-a951-47c5-86ed-3bb674f7be15" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("49b5d0ce-37d6-4563-8492-3cf358c1ffb1"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 22, 16, 54, 523, DateTimeKind.Utc).AddTicks(9477));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("65dcd066-b208-472e-b52c-6a86fbb3545e"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 22, 16, 54, 523, DateTimeKind.Utc).AddTicks(9470));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8c4d30cc-faa0-4d60-8c41-f055a6df1d91"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 22, 16, 54, 523, DateTimeKind.Utc).AddTicks(9483));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c8a7f282-84b9-41d1-b99a-8e4ea6714e4e"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 22, 16, 54, 523, DateTimeKind.Utc).AddTicks(9463));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("db52dc2f-50b7-4fc0-b27f-cb98e88ccff1"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 22, 16, 54, 523, DateTimeKind.Utc).AddTicks(9474));
        }
    }
}
