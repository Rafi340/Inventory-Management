using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifySalesTableSeedAccountData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountNo",
                table: "Sales",
                newName: "AccountId");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountTypeId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HolderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "AccountNumber", "AccountTypeId", "Balance", "BankName", "BranchName", "CreatedAt", "HolderName", "OpeningBalance", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Cash in Cash", 1, 0m, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0m, 1, null },
                    { 2, "SD Cash", 1, 0m, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0m, 1, null },
                    { 3, "TB Cash", 1, 0m, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0m, 1, null },
                    { 4, "016372383", 2, 1000m, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1000m, 1, null },
                    { 5, "019726323", 2, 100m, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 100m, 1, null },
                    { 6, "019724343", 2, 100m, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 100m, 1, null },
                    { 7, "018342342", 2, 100m, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 100m, 1, null },
                    { 8, "5446546445", 3, 100m, "IBBL", "Dholaikhal", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taveer", 100m, 1, null },
                    { 9, "56456444655", 3, 100m, "IBBL", "Uttara", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "khan", 100m, 1, null },
                    { 10, "3454365456", 3, 100m, "City Bank", "Ghulshan", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alomgir", 100m, 1, null },
                    { 11, "4398984343", 3, 100m, "City Bank", "Ghulshan", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Billal", 100m, 1, null }
                });

            migrationBuilder.InsertData(
                table: "AccountType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Cash" },
                    { 2, "Mobile Banking" },
                    { 3, "Bank" },
                    { 4, "Card" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "AccountType");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Sales",
                newName: "AccountNo");
        }
    }
}
