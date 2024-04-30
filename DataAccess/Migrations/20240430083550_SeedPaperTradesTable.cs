using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedPaperTradesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "StopPrice",
                table: "PaperTrades",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "FirstTarget",
                table: "PaperTrades",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "EntryPrice",
                table: "PaperTrades",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                table: "PaperTrades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenshotsUrls",
                table: "PaperTrades",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SideType",
                table: "PaperTrades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PaperTrades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Strategy",
                table: "PaperTrades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeFrame",
                table: "PaperTrades",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "PaperTrades",
                columns: new[] { "Id", "EntryPrice", "EntryTime", "ExitPrice", "ExitTime", "Fee", "FirstTarget", "Loss", "OrderType", "Profit", "ScreenshotsUrls", "SecondTarget", "SideType", "Status", "StopPrice", "Strategy", "Symbol", "TimeFrame", "TradeDuration", "TriggerPrice" },
                values: new object[] { 1, null, null, null, null, null, null, null, null, null, "[\"~/img/myimg/1.png\",\"~/img/myimg/2.png\",\"~/img/myimg/3.png\"]", null, null, null, null, 0, "BTCUSD", 1, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaperTrades",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "PaperTrades");

            migrationBuilder.DropColumn(
                name: "ScreenshotsUrls",
                table: "PaperTrades");

            migrationBuilder.DropColumn(
                name: "SideType",
                table: "PaperTrades");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PaperTrades");

            migrationBuilder.DropColumn(
                name: "Strategy",
                table: "PaperTrades");

            migrationBuilder.DropColumn(
                name: "TimeFrame",
                table: "PaperTrades");

            migrationBuilder.AlterColumn<double>(
                name: "StopPrice",
                table: "PaperTrades",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "FirstTarget",
                table: "PaperTrades",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "EntryPrice",
                table: "PaperTrades",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
