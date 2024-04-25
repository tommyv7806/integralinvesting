using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class NotReallySure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioStocks_PortfolioAssets_PortfolioAssetId",
                table: "PortfolioStocks");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioAssetId",
                table: "PortfolioStocks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioStocks_PortfolioAssets_PortfolioAssetId",
                table: "PortfolioStocks",
                column: "PortfolioAssetId",
                principalTable: "PortfolioAssets",
                principalColumn: "PortfolioAssetId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioStocks_PortfolioAssets_PortfolioAssetId",
                table: "PortfolioStocks");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioAssetId",
                table: "PortfolioStocks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioStocks_PortfolioAssets_PortfolioAssetId",
                table: "PortfolioStocks",
                column: "PortfolioAssetId",
                principalTable: "PortfolioAssets",
                principalColumn: "PortfolioAssetId");
        }
    }
}
