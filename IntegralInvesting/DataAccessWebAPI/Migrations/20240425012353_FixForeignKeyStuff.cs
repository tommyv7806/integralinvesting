using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeyStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioStocks_Portfolios_PortfolioId",
                table: "PortfolioStocks");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioStocks_PortfolioId",
                table: "PortfolioStocks");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "PortfolioStocks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "PortfolioStocks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioStocks_PortfolioId",
                table: "PortfolioStocks",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioStocks_Portfolios_PortfolioId",
                table: "PortfolioStocks",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "PortfolioId");
        }
    }
}
