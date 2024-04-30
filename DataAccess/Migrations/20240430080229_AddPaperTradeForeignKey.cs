using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPaperTradeForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaperTradeId",
                table: "Journals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Journals_PaperTradeId",
                table: "Journals",
                column: "PaperTradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_PaperTrades_PaperTradeId",
                table: "Journals",
                column: "PaperTradeId",
                principalTable: "PaperTrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journals_PaperTrades_PaperTradeId",
                table: "Journals");

            migrationBuilder.DropIndex(
                name: "IX_Journals_PaperTradeId",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "PaperTradeId",
                table: "Journals");
        }
    }
}
