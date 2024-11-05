using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtColumn2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "ResearchFirstBarPullbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "PaperTrades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "ResearchFirstBarPullbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "GETDATE()",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "PaperTrades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "GETDATE()",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
