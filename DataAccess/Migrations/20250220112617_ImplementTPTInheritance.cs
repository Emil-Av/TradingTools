using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ImplementTPTInheritance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Journals_JournalId",
                table: "Trades");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_ResearchFirstBarPullbacks_ResearchId",
                table: "Trades");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_SamplesSizes_SampleSizeId",
                table: "Trades");

            migrationBuilder.DropTable(
                name: "ResearchFirstBarPullbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trades",
                table: "Trades");

            migrationBuilder.RenameTable(
                name: "Trades",
                newName: "BaseTrades");

            migrationBuilder.RenameIndex(
                name: "IX_Trades_SampleSizeId",
                table: "BaseTrades",
                newName: "IX_BaseTrades_SampleSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_Trades_ResearchId",
                table: "BaseTrades",
                newName: "IX_BaseTrades_ResearchId");

            migrationBuilder.RenameIndex(
                name: "IX_Trades_JournalId",
                table: "BaseTrades",
                newName: "IX_BaseTrades_JournalId");

            migrationBuilder.AlterColumn<int>(
                name: "ResearchId",
                table: "BaseTrades",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaseTrades",
                table: "BaseTrades",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseTrades_BaseTrades_ResearchId",
                table: "BaseTrades",
                column: "ResearchId",
                principalTable: "BaseTrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseTrades_Journals_JournalId",
                table: "BaseTrades",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseTrades_SampleSizes_SampleSizeId",
                table: "BaseTrades",
                column: "SampleSizeId",
                principalTable: "SampleSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseTrades_BaseTrades_ResearchId",
                table: "BaseTrades");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseTrades_Journals_JournalId",
                table: "BaseTrades");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseTrades_SampleSizes_SampleSizeId",
                table: "BaseTrades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BaseTrades",
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

            migrationBuilder.RenameTable(
                name: "BaseTrades",
                newName: "Trades");

            migrationBuilder.RenameIndex(
                name: "IX_BaseTrades_SampleSizeId",
                table: "Trades",
                newName: "IX_Trades_SampleSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_BaseTrades_ResearchId",
                table: "Trades",
                newName: "IX_Trades_ResearchId");

            migrationBuilder.RenameIndex(
                name: "IX_BaseTrades_JournalId",
                table: "Trades",
                newName: "IX_Trades_JournalId");

            migrationBuilder.AlterColumn<int>(
                name: "ResearchId",
                table: "Trades",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trades",
                table: "Trades",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ResearchFirstBarPullbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JournalId = table.Column<int>(type: "int", nullable: true),
                    SampleSizeId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryPrice = table.Column<double>(type: "float", nullable: true),
                    ExitPrice = table.Column<double>(type: "float", nullable: true),
                    Fee = table.Column<double>(type: "float", nullable: true),
                    FullATRMarketGaveSmth = table.Column<bool>(type: "bit", nullable: false),
                    FullATRMaxRR = table.Column<int>(type: "int", nullable: false),
                    FullATROneToOneHitOn = table.Column<int>(type: "int", nullable: false),
                    Is4HTrending = table.Column<bool>(type: "bit", nullable: false),
                    IsBreakeven = table.Column<bool>(type: "bit", nullable: false),
                    IsDTrending = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryAfter3To5Bars = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryAfter5Bars = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryAfteriBar = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryAtPreviousSwingOnTrigger = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryBeforePreviousSwingOn4H = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryBeforePreviousSwingOnD = table.Column<bool>(type: "bit", nullable: false),
                    IsEntryBeforePreviousSwingOnTrigger = table.Column<bool>(type: "bit", nullable: false),
                    IsFullATRBreakeven = table.Column<bool>(type: "bit", nullable: false),
                    IsFullATRLoss = table.Column<bool>(type: "bit", nullable: false),
                    IsFullATROneToFiveHit = table.Column<bool>(type: "bit", nullable: false),
                    IsFullATROneToThreeHit = table.Column<bool>(type: "bit", nullable: false),
                    IsMomentumTrade = table.Column<bool>(type: "bit", nullable: false),
                    IsOneToFiveHit = table.Column<bool>(type: "bit", nullable: false),
                    IsOneToThreeHit = table.Column<bool>(type: "bit", nullable: false),
                    IsSignalBarInTradeDirection = table.Column<bool>(type: "bit", nullable: false),
                    IsSignalBarStrongReversal = table.Column<bool>(type: "bit", nullable: false),
                    IsTrendTrade = table.Column<bool>(type: "bit", nullable: false),
                    IsTriggerTrending = table.Column<bool>(type: "bit", nullable: false),
                    MarketGaveSmth = table.Column<bool>(type: "bit", nullable: false),
                    MaxRR = table.Column<int>(type: "int", nullable: false),
                    OneToOneHitOn = table.Column<int>(type: "int", nullable: false),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    Outcome = table.Column<int>(type: "int", nullable: false),
                    PnL = table.Column<double>(type: "float", nullable: true),
                    ScreenshotsUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SideType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StopPrice = table.Column<double>(type: "float", nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Targets = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    TradeRating = table.Column<int>(type: "int", nullable: false),
                    TradeType = table.Column<int>(type: "int", nullable: false),
                    TriggerPrice = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchFirstBarPullbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchFirstBarPullbacks_Journals_JournalId",
                        column: x => x.JournalId,
                        principalTable: "Journals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ResearchFirstBarPullbacks_SampleSizes_SampleSizeId",
                        column: x => x.SampleSizeId,
                        principalTable: "SampleSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResearchFirstBarPullbacks_JournalId",
                table: "ResearchFirstBarPullbacks",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchFirstBarPullbacks_SampleSizeId",
                table: "ResearchFirstBarPullbacks",
                column: "SampleSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Journals_JournalId",
                table: "Trades",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_ResearchFirstBarPullbacks_ResearchId",
                table: "Trades",
                column: "ResearchId",
                principalTable: "ResearchFirstBarPullbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_SamplesSizes_SampleSizeId",
                table: "Trades",
                column: "SampleSizeId",
                principalTable: "SampleSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
