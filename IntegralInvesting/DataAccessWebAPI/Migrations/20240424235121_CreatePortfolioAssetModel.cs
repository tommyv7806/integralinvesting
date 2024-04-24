using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreatePortfolioAssetModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortfolioAssetId",
                table: "PortfolioStocks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PortfolioAssets",
                columns: table => new
                {
                    PortfolioAssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortfolioId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    NumberOfShares = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioAssets", x => x.PortfolioAssetId);
                    table.ForeignKey(
                        name: "FK_PortfolioAssets_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "PortfolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioStocks_PortfolioAssetId",
                table: "PortfolioStocks",
                column: "PortfolioAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioAssets_PortfolioId",
                table: "PortfolioAssets",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioStocks_PortfolioAssets_PortfolioAssetId",
                table: "PortfolioStocks",
                column: "PortfolioAssetId",
                principalTable: "PortfolioAssets",
                principalColumn: "PortfolioAssetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioStocks_PortfolioAssets_PortfolioAssetId",
                table: "PortfolioStocks");

            migrationBuilder.DropTable(
                name: "PortfolioAssets");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioStocks_PortfolioAssetId",
                table: "PortfolioStocks");

            migrationBuilder.DropColumn(
                name: "PortfolioAssetId",
                table: "PortfolioStocks");
        }
    }
}
