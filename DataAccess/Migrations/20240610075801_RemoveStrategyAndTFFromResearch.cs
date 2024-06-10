using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStrategyAndTFFromResearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Strategy",
                table: "Research");

            migrationBuilder.RenameColumn(
                name: "TimeFrame",
                table: "Research",
                newName: "SideType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SideType",
                table: "Research",
                newName: "TimeFrame");

            migrationBuilder.AddColumn<int>(
                name: "Strategy",
                table: "Research",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
