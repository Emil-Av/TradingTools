using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameSearchFirstPullbackTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResearchFirstBarPullback_SampleSizes_SampleSizeId",
                table: "ResearchFirstBarPullback");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResearchFirstBarPullback",
                table: "ResearchFirstBarPullback");

            migrationBuilder.RenameTable(
                name: "ResearchFirstBarPullback",
                newName: "ResearchFirstBarPullbacks");

            migrationBuilder.RenameIndex(
                name: "IX_ResearchFirstBarPullback_SampleSizeId",
                table: "ResearchFirstBarPullbacks",
                newName: "IX_ResearchFirstBarPullbacks_SampleSizeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResearchFirstBarPullbacks",
                table: "ResearchFirstBarPullbacks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchFirstBarPullbacks_SampleSizes_SampleSizeId",
                table: "ResearchFirstBarPullbacks",
                column: "SampleSizeId",
                principalTable: "SampleSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResearchFirstBarPullbacks_SampleSizes_SampleSizeId",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResearchFirstBarPullbacks",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.RenameTable(
                name: "ResearchFirstBarPullbacks",
                newName: "ResearchFirstBarPullback");

            migrationBuilder.RenameIndex(
                name: "IX_ResearchFirstBarPullbacks_SampleSizeId",
                table: "ResearchFirstBarPullback",
                newName: "IX_ResearchFirstBarPullback_SampleSizeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResearchFirstBarPullback",
                table: "ResearchFirstBarPullback",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchFirstBarPullback_SampleSizes_SampleSizeId",
                table: "ResearchFirstBarPullback",
                column: "SampleSizeId",
                principalTable: "SampleSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
