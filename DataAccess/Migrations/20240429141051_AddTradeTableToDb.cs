using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTradeTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Instrument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerPrice = table.Column<double>(type: "float", nullable: true),
                    EntryPrice = table.Column<double>(type: "float", nullable: false),
                    StopPrice = table.Column<double>(type: "float", nullable: false),
                    FirstTarget = table.Column<double>(type: "float", nullable: false),
                    SecondTarget = table.Column<double>(type: "float", nullable: true),
                    ExitPrice = table.Column<double>(type: "float", nullable: true),
                    Profit = table.Column<double>(type: "float", nullable: true),
                    Loss = table.Column<double>(type: "float", nullable: true),
                    Fee = table.Column<double>(type: "float", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExitTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TradeDuration = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trades");
        }
    }
}
