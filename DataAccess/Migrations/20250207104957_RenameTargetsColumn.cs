using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameTargetsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Targets",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "Targets",
                table: "PaperTrades");

            migrationBuilder.AddColumn<string>(
                name: "TargetsJson",
                table: "ResearchFirstBarPullbacks",
                type: "NVARCHAR(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetsJson",
                table: "PaperTrades",
                type: "NVARCHAR(MAX)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetsJson",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "TargetsJson",
                table: "PaperTrades");

            migrationBuilder.AddColumn<string>(
                name: "Targets",
                table: "ResearchFirstBarPullbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Targets",
                table: "PaperTrades",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
