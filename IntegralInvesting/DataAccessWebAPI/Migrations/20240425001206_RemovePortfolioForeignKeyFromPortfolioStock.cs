using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovePortfolioForeignKeyFromPortfolioStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioStocks_Portfolios_PortfolioId",
                table: "PortfolioStocks");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioId",
                table: "PortfolioStocks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioStocks_Portfolios_PortfolioId",
                table: "PortfolioStocks",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "PortfolioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioStocks_Portfolios_PortfolioId",
                table: "PortfolioStocks");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioId",
                table: "PortfolioStocks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioStocks_Portfolios_PortfolioId",
                table: "PortfolioStocks",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "PortfolioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
