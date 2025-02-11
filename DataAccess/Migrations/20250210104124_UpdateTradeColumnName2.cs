using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTradeColumnName2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaperTrades_Journals_JournalId",
                table: "PaperTrades");

            migrationBuilder.DropForeignKey(
                name: "FK_PaperTrades_SamplesSizes_SampleSizeId",
                table: "PaperTrades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaperTrades",
                table: "PaperTrades");

            migrationBuilder.RenameTable(
                name: "PaperTrades",
                newName: "Trades");

            migrationBuilder.RenameIndex(
                name: "IX_PaperTrades_SampleSizeId",
                table: "Trades",
                newName: "IX_Trades_SampleSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_PaperTrades_JournalId",
                table: "Trades",
                newName: "IX_Trades_JournalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trades",
                table: "Trades",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Journals_JournalId",
                table: "Trades",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_SamplesSizes_SampleSizeId",
                table: "Trades",
                column: "SampleSizeId",
                principalTable: "SampleSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Journals_JournalId",
                table: "Trades");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_SampleSizes_SampleSizeId",
                table: "Trades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trades",
                table: "Trades");

            migrationBuilder.RenameTable(
                name: "Trades",
                newName: "PaperTrades");

            migrationBuilder.RenameIndex(
                name: "IX_Trades_SampleSizeId",
                table: "PaperTrades",
                newName: "IX_PaperTrades_SampleSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_Trades_JournalId",
                table: "PaperTrades",
                newName: "IX_PaperTrades_JournalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaperTrades",
                table: "PaperTrades",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaperTrades_Journals_JournalId",
                table: "PaperTrades",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaperTrades_SampleSizes_SampleSizeId",
                table: "PaperTrades",
                column: "SampleSizeId",
                principalTable: "SampleSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
