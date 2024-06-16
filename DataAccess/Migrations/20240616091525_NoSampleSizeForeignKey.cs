using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NoSampleSizeForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SampleSizeId",
                table: "Research",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Research_SampleSizeId",
                table: "Research",
                column: "SampleSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Research_SampleSizes_SampleSizeId",
                table: "Research",
                column: "SampleSizeId",
                principalTable: "SampleSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
