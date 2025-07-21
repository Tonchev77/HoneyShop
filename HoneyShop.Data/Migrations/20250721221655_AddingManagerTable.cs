#nullable disable

namespace HoneyShop.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class AddingManagerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                table: "Warehouses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                table: "OrderStatuses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Manager identifier"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Manager's user entity")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                },
                comment: "Manager in the system");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "15167365-502c-42be-9f14-3e623c2e465e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "2247c65e-6782-44e2-93ee-0d8d6caf771e", "AQAAAAIAAYagAAAAEHjWFnP5ny6LB9Hr4cNgsn0D0yyWhwrGS1BrKcQK1Yrzz/FUa/OQ8J+c0xD8lV+FIg==", "cd750359-a951-47c5-86ed-3bb674f7be15", "admin@honeyshop.com" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("1fbc3a2e-234a-4f9c-a6d8-f55a388ba5a7"),
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6b74e49c-8bfb-4c3d-b91e-3d5b441e9d13"),
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9a3ef5c7-7db9-4f4c-98c2-88c772cf8e91"),
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a4f0a2bc-b3de-45ac-a62b-5d0100329a6c"),
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c70e8376-0ed9-4265-94e3-b9e80b7cf42e"),
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("49b5d0ce-37d6-4563-8492-3cf358c1ffb1"),
                columns: new[] { "CreatedAt", "ManagerId" },
                values: new object[] { new DateTime(2025, 7, 21, 22, 16, 54, 523, DateTimeKind.Utc).AddTicks(9477), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("65dcd066-b208-472e-b52c-6a86fbb3545e"),
                columns: new[] { "CreatedAt", "ManagerId" },
                values: new object[] { new DateTime(2025, 7, 21, 22, 16, 54, 523, DateTimeKind.Utc).AddTicks(9470), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8c4d30cc-faa0-4d60-8c41-f055a6df1d91"),
                columns: new[] { "CreatedAt", "ManagerId" },
                values: new object[] { new DateTime(2025, 7, 21, 22, 16, 54, 523, DateTimeKind.Utc).AddTicks(9483), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c8a7f282-84b9-41d1-b99a-8e4ea6714e4e"),
                columns: new[] { "CreatedAt", "ManagerId" },
                values: new object[] { new DateTime(2025, 7, 21, 22, 16, 54, 523, DateTimeKind.Utc).AddTicks(9463), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("db52dc2f-50b7-4fc0-b27f-cb98e88ccff1"),
                columns: new[] { "CreatedAt", "ManagerId" },
                values: new object[] { new DateTime(2025, 7, 21, 22, 16, 54, 523, DateTimeKind.Utc).AddTicks(9474), null });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_ManagerId",
                table: "Warehouses",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ManagerId",
                table: "Products",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_ManagerId",
                table: "OrderStatuses",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ManagerId",
                table: "Categories",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_UserId",
                table: "Managers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Managers_ManagerId",
                table: "Categories",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatuses_Managers_ManagerId",
                table: "OrderStatuses",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Managers_ManagerId",
                table: "Products",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Managers_ManagerId",
                table: "Warehouses",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Managers_ManagerId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatuses_Managers_ManagerId",
                table: "OrderStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Managers_ManagerId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Managers_ManagerId",
                table: "Warehouses");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_ManagerId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Products_ManagerId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatuses_ManagerId",
                table: "OrderStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ManagerId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "15167365-502c-42be-9f14-3e623c2e465e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "404468ad-679f-4d60-b807-748862dab028", "AQAAAAIAAYagAAAAEKQC4MvE4VhHK6/GjbRof29eehVsOWuoexbYiUs722xLjU+xO/dKeofgEMODoN8nYA==", "f4222559-0e8e-4efa-9ed3-6a313c82916c", "admin@recipesharing.com" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("49b5d0ce-37d6-4563-8492-3cf358c1ffb1"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 17, 6, 48, 526, DateTimeKind.Utc).AddTicks(6431));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("65dcd066-b208-472e-b52c-6a86fbb3545e"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 17, 6, 48, 526, DateTimeKind.Utc).AddTicks(6422));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8c4d30cc-faa0-4d60-8c41-f055a6df1d91"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 17, 6, 48, 526, DateTimeKind.Utc).AddTicks(6434));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c8a7f282-84b9-41d1-b99a-8e4ea6714e4e"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 17, 6, 48, 526, DateTimeKind.Utc).AddTicks(6417));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("db52dc2f-50b7-4fc0-b27f-cb98e88ccff1"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 17, 6, 48, 526, DateTimeKind.Utc).AddTicks(6427));
        }
    }
}
