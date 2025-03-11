using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTPTInheritance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseTrades_BaseTrades_ResearchId",
                table: "BaseTrades");

            migrationBuilder.DropIndex(
                name: "IX_BaseTrades_ResearchId",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "FullATRMarketGaveSmth",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "FullATRMaxRR",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "FullATROneToOneHitOn",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "Is4HTrending",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsBreakeven",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsDTrending",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsEntryAfter3To5Bars",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsEntryAfter5Bars",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsEntryAfteriBar",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsEntryAtPreviousSwingOnTrigger",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsEntryBeforePreviousSwingOn4H",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsEntryBeforePreviousSwingOnD",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsEntryBeforePreviousSwingOnTrigger",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsFullATRBreakeven",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsFullATRLoss",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsFullATROneToFiveHit",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsFullATROneToThreeHit",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsMomentumTrade",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsOneToFiveHit",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsOneToThreeHit",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsSignalBarInTradeDirection",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsSignalBarStrongReversal",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsTrendTrade",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "IsTriggerTrending",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "MarketGaveSmth",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "MaxRR",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "OneToOneHitOn",
                table: "BaseTrades");

            migrationBuilder.DropColumn(
                name: "ResearchId",
                table: "BaseTrades");

            migrationBuilder.CreateTable(
                name: "ResearchFirstBarPullbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    OneToOneHitOn = table.Column<int>(type: "int", nullable: false),
                    IsOneToThreeHit = table.Column<bool>(type: "bit", nullable: false),
                    IsOneToFiveHit = table.Column<bool>(type: "bit", nullable: false),
                    IsBreakeven = table.Column<bool>(type: "bit", nullable: false),
                    MaxRR = table.Column<int>(type: "int", nullable: false),
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
                    FullATROneToOneHitOn = table.Column<int>(type: "int", nullable: false),
                    IsFullATROneToThreeHit = table.Column<bool>(type: "bit", nullable: false),
                    IsFullATROneToFiveHit = table.Column<bool>(type: "bit", nullable: false),
                    IsFullATRBreakeven = table.Column<bool>(type: "bit", nullable: false),
                    IsFullATRLoss = table.Column<bool>(type: "bit", nullable: false),
                    FullATRMaxRR = table.Column<int>(type: "int", nullable: false),
                    FullATRMarketGaveSmth = table.Column<bool>(type: "bit", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchFirstBarPullbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchFirstBarPullbacks_BaseTrades_Id",
                        column: x => x.Id,
                        principalTable: "BaseTrades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ResearchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trades_BaseTrades_Id",
                        column: x => x.Id,
                        principalTable: "BaseTrades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trades_ResearchFirstBarPullbacks_ResearchId",
                        column: x => x.ResearchId,
                        principalTable: "ResearchFirstBarPullbacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trades_ResearchId",
                table: "Trades",
                column: "ResearchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trades");

            migrationBuilder.DropTable(
                name: "ResearchFirstBarPullbacks");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "BaseTrades",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "BaseTrades",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "FullATRMarketGaveSmth",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FullATRMaxRR",
                table: "BaseTrades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FullATROneToOneHitOn",
                table: "BaseTrades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is4HTrending",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBreakeven",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDTrending",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEntryAfter3To5Bars",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEntryAfter5Bars",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEntryAfteriBar",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEntryAtPreviousSwingOnTrigger",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEntryBeforePreviousSwingOn4H",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEntryBeforePreviousSwingOnD",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEntryBeforePreviousSwingOnTrigger",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullATRBreakeven",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullATRLoss",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullATROneToFiveHit",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullATROneToThreeHit",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMomentumTrade",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOneToFiveHit",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOneToThreeHit",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSignalBarInTradeDirection",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSignalBarStrongReversal",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrendTrade",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTriggerTrending",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MarketGaveSmth",
                table: "BaseTrades",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxRR",
                table: "BaseTrades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OneToOneHitOn",
                table: "BaseTrades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResearchId",
                table: "BaseTrades",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseTrades_ResearchId",
                table: "BaseTrades",
                column: "ResearchId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseTrades_BaseTrades_ResearchId",
                table: "BaseTrades",
                column: "ResearchId",
                principalTable: "BaseTrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
