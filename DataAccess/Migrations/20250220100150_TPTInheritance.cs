using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TPTInheritance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResearchFirstBarPullbacks_Journals_JournalId",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_ResearchFirstBarPullbacks_SampleSizes_SampleSizeId",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Journals_JournalId",
                table: "Trades");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_ResearchFirstBarPullbacks_ResearchId",
                table: "Trades");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_SampleSizes_SampleSizeId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_JournalId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_SampleSizeId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_ResearchFirstBarPullbacks_JournalId",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropIndex(
                name: "IX_ResearchFirstBarPullbacks_SampleSizeId",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "EntryPrice",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "ExitPrice",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "JournalId",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Outcome",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "PnL",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "SampleSizeId",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "ScreenshotsUrls",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "SideType",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "StopPrice",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "Targets",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "TradeRating",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "TradeType",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "TriggerPrice",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "EntryPrice",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "ExitPrice",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "JournalId",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "Outcome",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "PnL",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "SampleSizeId",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "ScreenshotsUrls",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "SideType",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "StopPrice",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "Targets",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "TradeRating",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "TradeType",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "TriggerPrice",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Trades",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "BaseTrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TriggerPrice = table.Column<double>(type: "float", nullable: true),
                    EntryPrice = table.Column<double>(type: "float", nullable: true),
                    StopPrice = table.Column<double>(type: "float", nullable: true),
                    ExitPrice = table.Column<double>(type: "float", nullable: true),
                    Targets = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    PnL = table.Column<double>(type: "float", nullable: true),
                    Fee = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SideType = table.Column<int>(type: "int", nullable: false),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    TradeRating = table.Column<int>(type: "int", nullable: false),
                    TradeType = table.Column<int>(type: "int", nullable: false),
                    Outcome = table.Column<int>(type: "int", nullable: false),
                    ScreenshotsUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SampleSizeId = table.Column<int>(type: "int", nullable: false),
                    JournalId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseTrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseTrades_Journals_JournalId",
                        column: x => x.JournalId,
                        principalTable: "Journals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseTrades_SampleSizes_SampleSizeId",
                        column: x => x.SampleSizeId,
                        principalTable: "SampleSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseTrades_JournalId",
                table: "BaseTrades",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTrades_SampleSizeId",
                table: "BaseTrades",
                column: "SampleSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchFirstBarPullbacks_BaseTrades_Id",
                table: "ResearchFirstBarPullbacks",
                column: "Id",
                principalTable: "BaseTrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_BaseTrades_Id",
                table: "Trades",
                column: "Id",
                principalTable: "BaseTrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_ResearchFirstBarPullbacks_ResearchId",
                table: "Trades",
                column: "ResearchId",
                principalTable: "ResearchFirstBarPullbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResearchFirstBarPullbacks_BaseTrades_Id",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_BaseTrades_Id",
                table: "Trades");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_ResearchFirstBarPullbacks_ResearchId",
                table: "Trades");

            migrationBuilder.DropTable(
                name: "BaseTrades");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Trades",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Trades",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "EntryPrice",
                table: "Trades",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExitPrice",
                table: "Trades",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Fee",
                table: "Trades",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JournalId",
                table: "Trades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                table: "Trades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Outcome",
                table: "Trades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PnL",
                table: "Trades",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SampleSizeId",
                table: "Trades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ScreenshotsUrls",
                table: "Trades",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SideType",
                table: "Trades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Trades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "StopPrice",
                table: "Trades",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Trades",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Targets",
                table: "Trades",
                type: "NVARCHAR(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TradeRating",
                table: "Trades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TradeType",
                table: "Trades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TriggerPrice",
                table: "Trades",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ResearchFirstBarPullbacks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "EntryPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExitPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Fee",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JournalId",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Outcome",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PnL",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SampleSizeId",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ScreenshotsUrls",
                table: "ResearchFirstBarPullbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SideType",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "StopPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "ResearchFirstBarPullbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Targets",
                table: "ResearchFirstBarPullbacks",
                type: "NVARCHAR(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TradeRating",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TradeType",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TriggerPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trades_JournalId",
                table: "Trades",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_SampleSizeId",
                table: "Trades",
                column: "SampleSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchFirstBarPullbacks_JournalId",
                table: "ResearchFirstBarPullbacks",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchFirstBarPullbacks_SampleSizeId",
                table: "ResearchFirstBarPullbacks",
                column: "SampleSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchFirstBarPullbacks_Journals_JournalId",
                table: "ResearchFirstBarPullbacks",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchFirstBarPullbacks_SampleSizes_SampleSizeId",
                table: "ResearchFirstBarPullbacks",
                column: "SampleSizeId",
                principalTable: "SampleSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Trades_SampleSizes_SampleSizeId",
                table: "Trades",
                column: "SampleSizeId",
                principalTable: "SampleSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
