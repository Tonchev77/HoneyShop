#nullable disable

namespace HoneyShop.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;
    /// <inheritdoc />
    public partial class AddedCreatedAtForApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "15167365-502c-42be-9f14-3e623c2e465e",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e26c6d85-a622-424b-81a0-d6179cccf077", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAEON2+g43AUrOTSsNe1QjTaxmpxgwJHD14BQpnwqPOot8VCbmEAR/tITAidCPwHKG8Q==", "bfad6287-afa1-4e6a-9261-20e6c740005a" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("49b5d0ce-37d6-4563-8492-3cf358c1ffb1"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 22, 14, 51, 949, DateTimeKind.Utc).AddTicks(1310));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("65dcd066-b208-472e-b52c-6a86fbb3545e"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 22, 14, 51, 949, DateTimeKind.Utc).AddTicks(1301));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8c4d30cc-faa0-4d60-8c41-f055a6df1d91"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 22, 14, 51, 949, DateTimeKind.Utc).AddTicks(1313));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c8a7f282-84b9-41d1-b99a-8e4ea6714e4e"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 22, 14, 51, 949, DateTimeKind.Utc).AddTicks(1295));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("db52dc2f-50b7-4fc0-b27f-cb98e88ccff1"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 22, 14, 51, 949, DateTimeKind.Utc).AddTicks(1306));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "15167365-502c-42be-9f14-3e623c2e465e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0dd0d4e8-69a2-4a65-a04c-cab80edf77c2", "AQAAAAIAAYagAAAAEIHpRDO+tPNlRCs80OV46T8wdKq+HvVHBMBxwh8k2BC+5pZNPTKb3RESR0rqe89qKA==", "e0720c8b-d544-484b-a754-9f2c0f83e0df" });

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
        }
    }
}
