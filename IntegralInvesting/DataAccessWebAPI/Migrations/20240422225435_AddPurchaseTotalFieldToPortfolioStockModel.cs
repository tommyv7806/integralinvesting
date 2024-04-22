using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseTotalFieldToPortfolioStockModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PurchaseTotal",
                table: "PortfolioStocks",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseTotal",
                table: "PortfolioStocks");
        }
    }
}
