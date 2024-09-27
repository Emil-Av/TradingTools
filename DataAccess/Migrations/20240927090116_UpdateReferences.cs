using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "ReviewId",
                table: "SampleSizes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResearchId",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "JournalId",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResearchId",
                table: "PaperTrades",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "JournalId",
                table: "PaperTrades",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SampleSizes_ReviewId",
                table: "SampleSizes",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchFirstBarPullbacks_JournalId",
                table: "ResearchFirstBarPullbacks",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_PaperTrades_JournalId",
                table: "PaperTrades",
                column: "JournalId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaperTrades_Journals_JournalId",
                table: "PaperTrades",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchFirstBarPullbacks_Journals_JournalId",
                table: "ResearchFirstBarPullbacks",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleSizes_Reviews_ReviewId",
                table: "SampleSizes",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaperTrades_Journals_JournalId",
                table: "PaperTrades");

            migrationBuilder.DropForeignKey(
                name: "FK_ResearchFirstBarPullbacks_Journals_JournalId",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleSizes_Reviews_ReviewId",
                table: "SampleSizes");

            migrationBuilder.DropIndex(
                name: "IX_SampleSizes_ReviewId",
                table: "SampleSizes");

            migrationBuilder.DropIndex(
                name: "IX_ResearchFirstBarPullbacks_JournalId",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropIndex(
                name: "IX_PaperTrades_JournalId",
                table: "PaperTrades");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "SampleSizes");

            migrationBuilder.DropColumn(
                name: "JournalId",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "JournalId",
                table: "PaperTrades");

            migrationBuilder.AlterColumn<int>(
                name: "ResearchId",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResearchId",
                table: "PaperTrades",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
