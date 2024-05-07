using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSampleSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SampleSizeId",
                table: "PaperTrades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SampleSize",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Strategy = table.Column<int>(type: "int", nullable: false),
                    TimeFrame = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleSize", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "PaperTrades",
                keyColumn: "Id",
                keyValue: 1,
                column: "SampleSizeId",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaperTrades_SampleSizeId",
                table: "PaperTrades",
                column: "SampleSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaperTrades_SampleSize_SampleSizeId",
                table: "PaperTrades",
                column: "SampleSizeId",
                principalTable: "SampleSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaperTrades_SampleSize_SampleSizeId",
                table: "PaperTrades");

            migrationBuilder.DropTable(
                name: "SampleSize");

            migrationBuilder.DropIndex(
                name: "IX_PaperTrades_SampleSizeId",
                table: "PaperTrades");

            migrationBuilder.DropColumn(
                name: "SampleSizeId",
                table: "PaperTrades");
        }
    }
}
