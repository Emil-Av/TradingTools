using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedAt",
                table: "ResearchFirstBarPullbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "GETDATE()");

            migrationBuilder.AddColumn<string>(
                name: "CreatedAt",
                table: "PaperTrades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PaperTrades");
        }
    }
}
