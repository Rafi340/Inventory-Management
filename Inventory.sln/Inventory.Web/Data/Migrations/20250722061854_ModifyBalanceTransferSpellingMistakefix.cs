using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyBalanceTransferSpellingMistakefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderAcountTypeId",
                table: "BalanceTransfer",
                newName: "SenderAccountTypeId");

            migrationBuilder.RenameColumn(
                name: "ReceiverAcountTypeId",
                table: "BalanceTransfer",
                newName: "ReceiverAccountTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderAccountTypeId",
                table: "BalanceTransfer",
                newName: "SenderAcountTypeId");

            migrationBuilder.RenameColumn(
                name: "ReceiverAccountTypeId",
                table: "BalanceTransfer",
                newName: "ReceiverAcountTypeId");
        }
    }
}
