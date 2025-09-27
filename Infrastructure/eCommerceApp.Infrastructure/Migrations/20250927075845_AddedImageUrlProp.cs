using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedImageUrlProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedDate",
                value: new DateTime(2025, 9, 27, 7, 58, 44, 956, DateTimeKind.Utc).AddTicks(9605));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedDate",
                value: new DateTime(2025, 9, 27, 7, 58, 44, 956, DateTimeKind.Utc).AddTicks(9605));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 9, 27, 7, 58, 44, 956, DateTimeKind.Utc).AddTicks(9605), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000002"),
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 9, 27, 7, 58, 44, 956, DateTimeKind.Utc).AddTicks(9605), null });

            migrationBuilder.UpdateData(
                table: "Subcategories",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"),
                column: "CreatedDate",
                value: new DateTime(2025, 9, 27, 7, 58, 44, 956, DateTimeKind.Utc).AddTicks(9605));

            migrationBuilder.UpdateData(
                table: "Subcategories",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000002"),
                column: "CreatedDate",
                value: new DateTime(2025, 9, 27, 7, 58, 44, 956, DateTimeKind.Utc).AddTicks(9605));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"),
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 9, 55, 9, 25, DateTimeKind.Utc).AddTicks(7603));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"),
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 9, 55, 9, 25, DateTimeKind.Utc).AddTicks(7603));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000001"),
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 9, 55, 9, 25, DateTimeKind.Utc).AddTicks(7603));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000002"),
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 9, 55, 9, 25, DateTimeKind.Utc).AddTicks(7603));

            migrationBuilder.UpdateData(
                table: "Subcategories",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"),
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 9, 55, 9, 25, DateTimeKind.Utc).AddTicks(7603));

            migrationBuilder.UpdateData(
                table: "Subcategories",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000002"),
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 9, 55, 9, 25, DateTimeKind.Utc).AddTicks(7603));
        }
    }
}
