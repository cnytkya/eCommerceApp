using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eCommerceApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedSeedDataUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "64467c15-78d8-4ca9-bb02-03b271448f5d", null, "Admin", "ADMIN" },
                    { "7cf1551f-86f3-4aa0-95f7-2e5e247ea44a", null, "User", "USER" }
                });
            
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Bio", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Email", "EmailConfirmed", "Fullname", "IsActive", "IsDeleted", "LastLoginDate", "Location", "LockoutEnabled", "LockoutEnd", "ModifiedBy", "ModifiedDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilImgUrl", "RegistrationDate", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "23449f44-7d82-48dd-8884-08d0e3ac50ba", 0, null, "3b53bc68-d73a-414a-9b04-5bd2d18152f7", null, new DateTime(2025, 6, 28, 10, 27, 20, 61, DateTimeKind.Utc).AddTicks(410), null, new DateTime(2025, 6, 28, 10, 27, 20, 130, DateTimeKind.Utc).AddTicks(2202), "user@example.com", true, "Regular User", true, false, new DateTime(2025, 6, 28, 10, 27, 20, 61, DateTimeKind.Utc).AddTicks(410), null, false, null, null, new DateTime(2025, 6, 28, 10, 27, 20, 61, DateTimeKind.Utc).AddTicks(410), "USER@EXAMPLE.COM", "USER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEAvo4kLenEKgrpVdmj4s7aSPaNx8fRffDEazWvB6pVpYE+hpnU3+FPgBnj8uwvXY1w==", null, false, null, new DateTime(2025, 6, 28, 10, 27, 20, 61, DateTimeKind.Utc).AddTicks(410), "3ed36bef-896d-4096-a1f1-dc9e10f41302", false, "user@example.com" },
                    { "428f2117-b417-4fb8-a183-1eff589eac85", 0, null, "54711390-a12a-4682-ac74-517ed3a36956", null, new DateTime(2025, 6, 28, 10, 27, 20, 61, DateTimeKind.Utc).AddTicks(410), null, new DateTime(2025, 6, 28, 10, 27, 20, 64, DateTimeKind.Utc).AddTicks(2854), "admin@example.com", true, "Admin User", true, false, new DateTime(2025, 6, 28, 10, 27, 20, 61, DateTimeKind.Utc).AddTicks(410), null, false, null, null, new DateTime(2025, 6, 28, 10, 27, 20, 61, DateTimeKind.Utc).AddTicks(410), "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEMexPyu+LAcd4sF6vO9JgkNFuEY4BjkSDSQ2+ueJ/Ht1oIj4W2+Znm5ZeJTodCn/CA==", null, false, null, new DateTime(2025, 6, 28, 10, 27, 20, 61, DateTimeKind.Utc).AddTicks(410), "27c3a0ae-df8d-4c53-9b5b-88c3f23ece70", false, "admin@example.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "7cf1551f-86f3-4aa0-95f7-2e5e247ea44a", "23449f44-7d82-48dd-8884-08d0e3ac50ba" },
                    { "64467c15-78d8-4ca9-bb02-03b271448f5d", "428f2117-b417-4fb8-a183-1eff589eac85" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7cf1551f-86f3-4aa0-95f7-2e5e247ea44a", "23449f44-7d82-48dd-8884-08d0e3ac50ba" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "64467c15-78d8-4ca9-bb02-03b271448f5d", "428f2117-b417-4fb8-a183-1eff589eac85" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64467c15-78d8-4ca9-bb02-03b271448f5d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7cf1551f-86f3-4aa0-95f7-2e5e247ea44a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "23449f44-7d82-48dd-8884-08d0e3ac50ba");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "428f2117-b417-4fb8-a183-1eff589eac85");
        }
    }
}
