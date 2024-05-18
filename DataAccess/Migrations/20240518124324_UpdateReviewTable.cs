using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SampleSizeReview",
                table: "Reviews",
                newName: "Third");

            migrationBuilder.AddColumn<string>(
                name: "First",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Forth",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Second",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "First",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Forth",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Second",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "Third",
                table: "Reviews",
                newName: "SampleSizeReview");
        }
    }
}
