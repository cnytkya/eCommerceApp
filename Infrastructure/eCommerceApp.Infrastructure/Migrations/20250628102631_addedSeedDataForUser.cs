using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eCommerceApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedSeedDataForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6b791d3e-406c-4b8c-a188-f6a33451bc15", null, "Admin", "ADMIN" },
                    { "e15a4156-2867-4c9a-83c7-c9f0f1dc28e2", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Bio", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Email", "EmailConfirmed", "Fullname", "IsActive", "IsDeleted", "LastLoginDate", "Location", "LockoutEnabled", "LockoutEnd", "ModifiedBy", "ModifiedDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilImgUrl", "RegistrationDate", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "476ff7bb-d243-4b7f-a4c7-88360245b763", 0, null, "81b5bacc-3ec7-4e7d-b3b2-52924357ad45", null, new DateTime(2025, 6, 28, 10, 26, 30, 490, DateTimeKind.Utc).AddTicks(1845), null, new DateTime(2025, 6, 28, 10, 26, 30, 535, DateTimeKind.Utc).AddTicks(2498), "user@example.com", true, "Regular User", true, false, new DateTime(2025, 6, 28, 10, 26, 30, 490, DateTimeKind.Utc).AddTicks(1845), null, false, null, null, new DateTime(2025, 6, 28, 10, 26, 30, 490, DateTimeKind.Utc).AddTicks(1845), "USER@EXAMPLE.COM", "USER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEPierKHsiYIRwgiJVVCHFiv6RDJnlnmdQ10HHwJmbCfJVvGYFYjfGeXRuEhjaHFDrw==", null, false, null, new DateTime(2025, 6, 28, 10, 26, 30, 490, DateTimeKind.Utc).AddTicks(1845), "54fb603d-d4fb-40db-bcfd-6a6e7919bfd1", false, "user@example.com" },
                    { "8cc9641b-ffca-4409-9aa8-178cae8112ed", 0, null, "cccc3080-f456-4218-8c73-e33af7a6e78d", null, new DateTime(2025, 6, 28, 10, 26, 30, 490, DateTimeKind.Utc).AddTicks(1845), null, new DateTime(2025, 6, 28, 10, 26, 30, 491, DateTimeKind.Utc).AddTicks(9379), "admin@example.com", true, "Admin User", true, false, new DateTime(2025, 6, 28, 10, 26, 30, 490, DateTimeKind.Utc).AddTicks(1845), null, false, null, null, new DateTime(2025, 6, 28, 10, 26, 30, 490, DateTimeKind.Utc).AddTicks(1845), "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEMuu8pDn1Mv8ur4hj/4JmR/ZMzNQy8UNbEl/OLa/xsef4oLd4FoismeH8jxtJZJsKg==", null, false, null, new DateTime(2025, 6, 28, 10, 26, 30, 490, DateTimeKind.Utc).AddTicks(1845), "74c8a027-6087-4478-8f02-a072d05652a5", false, "admin@example.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "e15a4156-2867-4c9a-83c7-c9f0f1dc28e2", "476ff7bb-d243-4b7f-a4c7-88360245b763" },
                    { "6b791d3e-406c-4b8c-a188-f6a33451bc15", "8cc9641b-ffca-4409-9aa8-178cae8112ed" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e15a4156-2867-4c9a-83c7-c9f0f1dc28e2", "476ff7bb-d243-4b7f-a4c7-88360245b763" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "6b791d3e-406c-4b8c-a188-f6a33451bc15", "8cc9641b-ffca-4409-9aa8-178cae8112ed" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b791d3e-406c-4b8c-a188-f6a33451bc15");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e15a4156-2867-4c9a-83c7-c9f0f1dc28e2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "476ff7bb-d243-4b7f-a4c7-88360245b763");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8cc9641b-ffca-4409-9aa8-178cae8112ed");
        }
    }
}
