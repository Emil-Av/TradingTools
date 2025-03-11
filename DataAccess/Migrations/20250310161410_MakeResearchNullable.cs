using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MakeResearchNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_ResearchFirstBarPullbacks_ResearchId",
                table: "Trades");

            migrationBuilder.AlterColumn<int>(
                name: "ResearchId",
                table: "Trades",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_ResearchFirstBarPullbacks_ResearchId",
                table: "Trades",
                column: "ResearchId",
                principalTable: "ResearchFirstBarPullbacks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_ResearchFirstBarPullbacks_ResearchId",
                table: "Trades");

            migrationBuilder.AlterColumn<int>(
                name: "ResearchId",
                table: "Trades",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_ResearchFirstBarPullbacks_ResearchId",
                table: "Trades",
                column: "ResearchId",
                principalTable: "ResearchFirstBarPullbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
