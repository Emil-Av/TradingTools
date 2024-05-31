using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnResearchTrades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResearchTrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScreenshotsUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OneToOneHitOn = table.Column<int>(type: "int", nullable: false),
                    IsOneToThreeHit = table.Column<bool>(type: "bit", nullable: false),
                    IsOneToFiveHit = table.Column<bool>(type: "bit", nullable: false),
                    IsBreakeven = table.Column<bool>(type: "bit", nullable: false),
                    IsLoss = table.Column<bool>(type: "bit", nullable: false),
                    MaxRR = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MarketGaveSmth = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryAfter3To5Bars = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryAfter5Bars = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryAtPreviousSwingOnTrigger = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryBeforePreviousSwingOnTrigger = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryBeforePreviousSwingOn4H = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryBeforePreviousSwingOnD = table.Column<bool>(type: "bit", nullable: false),
                    IsMomentumTrade = table.Column<bool>(type: "bit", nullable: false),
                    IsTrendTrade = table.Column<bool>(type: "bit", nullable: false),
                    IsTriggerTrending = table.Column<bool>(type: "bit", nullable: false),
                    Is4HTrending = table.Column<bool>(type: "bit", nullable: false),
                    IsDTrending = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryAfteriBar = table.Column<bool>(type: "bit", nullable: false),
                    IsSignalBarStrongReversal = table.Column<bool>(type: "bit", nullable: false),
                    IsSignalBarInTradeDirection = table.Column<bool>(type: "bit", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeFrame = table.Column<int>(type: "int", nullable: false),
                    Strategy = table.Column<int>(type: "int", nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchTrades", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResearchTrades");
        }
    }
}
