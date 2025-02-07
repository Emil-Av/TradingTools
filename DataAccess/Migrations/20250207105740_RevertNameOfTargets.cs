using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RevertNameOfTargets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetsJson",
                table: "ResearchFirstBarPullbacks",
                newName: "Targets");

            migrationBuilder.RenameColumn(
                name: "TargetsJson",
                table: "PaperTrades",
                newName: "Targets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Targets",
                table: "ResearchFirstBarPullbacks",
                newName: "TargetsJson");

            migrationBuilder.RenameColumn(
                name: "Targets",
                table: "PaperTrades",
                newName: "TargetsJson");
        }
    }
}
