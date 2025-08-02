using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class RoleSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("23fe4e81-5015-42a0-8d76-d1f08c6b227a"), "7/1/2025 1:02:03 AM", "SuperAdmin", "SuperAdmin" },
                    { new Guid("5e76c76d-4cf6-4784-8796-fd9f5c8cf29d"), "8/1/2025 1:02:01 AM", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("23fe4e81-5015-42a0-8d76-d1f08c6b227a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5e76c76d-4cf6-4784-8796-fd9f5c8cf29d"));
        }
    }
}
