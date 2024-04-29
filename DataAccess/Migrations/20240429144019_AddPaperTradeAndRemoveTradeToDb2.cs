using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPaperTradeAndRemoveTradeToDb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Trades",
                table: "Trades");

            migrationBuilder.RenameTable(
                name: "Trades",
                newName: "PaperTrades");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaperTrades",
                table: "PaperTrades",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PaperTrades",
                table: "PaperTrades");

            migrationBuilder.RenameTable(
                name: "PaperTrades",
                newName: "Trades");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trades",
                table: "Trades",
                column: "Id");
        }
    }
}
