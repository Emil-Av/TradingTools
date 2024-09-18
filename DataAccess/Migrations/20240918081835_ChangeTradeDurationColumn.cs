using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTradeDurationColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TradeDuration",
                table: "ResearchFirstBarPullbacks",
                newName: "TradeDurationInCandles");

            migrationBuilder.RenameColumn(
                name: "TradeDuration",
                table: "PaperTrades",
                newName: "TradeDurationInCandles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TradeDurationInCandles",
                table: "ResearchFirstBarPullbacks",
                newName: "TradeDuration");

            migrationBuilder.RenameColumn(
                name: "TradeDurationInCandles",
                table: "PaperTrades",
                newName: "TradeDuration");
        }
    }
}
