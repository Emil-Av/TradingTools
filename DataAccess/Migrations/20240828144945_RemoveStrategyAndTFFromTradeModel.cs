using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStrategyAndTFFromTradeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntryTime",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "ExitTime",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "Strategy",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "TimeFrame",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "EntryTime",
                table: "PaperTrades");

            migrationBuilder.DropColumn(
                name: "ExitTime",
                table: "PaperTrades");

            migrationBuilder.DropColumn(
                name: "Strategy",
                table: "PaperTrades");

            migrationBuilder.DropColumn(
                name: "TimeFrame",
                table: "PaperTrades");

            migrationBuilder.AddColumn<int>(
                 name: "TradeDuration",
                 table: "ResearchFirstBarPullbacks",
                 type: "int",
                 nullable: false,
                 defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TradeDuration",
                table: "PaperTrades",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TradeDuration",
                table: "ResearchFirstBarPullbacks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryTime",
                table: "ResearchFirstBarPullbacks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExitTime",
                table: "ResearchFirstBarPullbacks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Strategy",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeFrame",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TradeDuration",
                table: "PaperTrades",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryTime",
                table: "PaperTrades",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExitTime",
                table: "PaperTrades",
                type: "datetime2",
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
        }
    }
}
