using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixingResearchIdReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResearchId",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.AlterColumn<double>(
                name: "TriggerPrice",
                table: "Trades",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "Targets",
                table: "Trades",
                type: "NVARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)");

            migrationBuilder.AlterColumn<double>(
                name: "StopPrice",
                table: "Trades",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "ResearchId",
                table: "Trades",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PnL",
                table: "Trades",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "Fee",
                table: "Trades",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "ExitPrice",
                table: "Trades",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "EntryPrice",
                table: "Trades",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Trades",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<double>(
                name: "TriggerPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<string>(
                name: "Targets",
                table: "ResearchFirstBarPullbacks",
                type: "NVARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)");

            migrationBuilder.AlterColumn<double>(
                name: "StopPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "PnL",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "Fee",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "ExitPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "EntryPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ResearchFirstBarPullbacks",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_ResearchId",
                table: "Trades",
                column: "ResearchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_ResearchFirstBarPullbacks_ResearchId",
                table: "Trades",
                column: "ResearchId",
                principalTable: "ResearchFirstBarPullbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_ResearchFirstBarPullbacks_ResearchId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_ResearchId",
                table: "Trades");

            migrationBuilder.AlterColumn<double>(
                name: "TriggerPrice",
                table: "Trades",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Targets",
                table: "Trades",
                type: "NVARCHAR(MAX)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "StopPrice",
                table: "Trades",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResearchId",
                table: "Trades",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "PnL",
                table: "Trades",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Fee",
                table: "Trades",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ExitPrice",
                table: "Trades",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "EntryPrice",
                table: "Trades",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Trades",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<double>(
                name: "TriggerPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Targets",
                table: "ResearchFirstBarPullbacks",
                type: "NVARCHAR(MAX)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "StopPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PnL",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Fee",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ExitPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "EntryPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ResearchFirstBarPullbacks",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "ResearchId",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: true);
        }
    }
}
