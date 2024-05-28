using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCalculatorSetupToUserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AccountSize",
                table: "UserSettings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExchSizeLimit",
                table: "UserSettings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxSlippage",
                table: "UserSettings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ScaleOut",
                table: "UserSettings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TradeFee",
                table: "UserSettings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TradeRisk",
                table: "UserSettings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountSize",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "ExchSizeLimit",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "MaxSlippage",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "ScaleOut",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "TradeFee",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "TradeRisk",
                table: "UserSettings");
        }
    }
}
