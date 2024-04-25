using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNumberOfSharesFromPortfolioAssetMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfShares",
                table: "PortfolioAssets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfShares",
                table: "PortfolioAssets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
