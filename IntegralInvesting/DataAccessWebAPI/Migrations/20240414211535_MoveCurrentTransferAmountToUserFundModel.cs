using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class MoveCurrentTransferAmountToUserFundModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentTransferAmount",
                table: "BankAccounts");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentTransferAmount",
                table: "UserFunds",
                type: "decimal(8,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentTransferAmount",
                table: "UserFunds");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentTransferAmount",
                table: "BankAccounts",
                type: "decimal(8,2)",
                nullable: true);
        }
    }
}
