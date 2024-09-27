using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSampleSizeFromReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_SampleSizes_SampleSizeId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_SampleSizeId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "SampleSizeId",
                table: "Reviews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SampleSizeId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_SampleSizeId",
                table: "Reviews",
                column: "SampleSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_SampleSizes_SampleSizeId",
                table: "Reviews",
                column: "SampleSizeId",
                principalTable: "SampleSizes",
                principalColumn: "Id");
        }
    }
}
