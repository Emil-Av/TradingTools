using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameResearchTrades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaperTrades",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PaperTrades",
                columns: new[] { "Id", "EntryPrice", "EntryTime", "ExitPrice", "ExitTime", "Fee", "FirstTarget", "Loss", "OrderType", "Profit", "SampleSizeId", "ScreenshotsUrls", "SecondTarget", "SideType", "Status", "StopPrice", "Strategy", "Symbol", "TimeFrame", "TradeDuration", "TriggerPrice" },
                values: new object[] { 1, null, null, null, null, null, null, null, null, null, 0, "[\"~/img/myimg/1.png\",\"~/img/myimg/2.png\",\"~/img/myimg/3.png\"]", null, null, null, null, 0, "BTCUSD", 1, null, null });
        }
    }
}
