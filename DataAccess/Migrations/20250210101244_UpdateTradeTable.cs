using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTradeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLoss",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "TradeDurationInCandles",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "IsLoss",
                table: "PaperTrades");

            migrationBuilder.DropColumn(
                name: "TradeDurationInCandles",
                table: "PaperTrades");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLoss",
                table: "ResearchFirstBarPullbacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TradeDurationInCandles",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: true,
                defaultValueSql: "0");

            migrationBuilder.AddColumn<bool>(
                name: "IsLoss",
                table: "PaperTrades",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TradeDurationInCandles",
                table: "PaperTrades",
                type: "int",
                nullable: true,
                defaultValueSql: "0");
        }
    }
}
