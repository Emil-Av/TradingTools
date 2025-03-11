using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixTPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseTrades_SampleSizes_SampleSizeId",
                table: "BaseTrades");

            migrationBuilder.DropForeignKey(
                name: "FK_ResearchFirstBarPullbacks_BaseTrades_Id",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "TradeType",
                table: "BaseTrades");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseTrades_SampleSizes_SampleSizeId",
                table: "BaseTrades",
                column: "SampleSizeId",
                principalTable: "SampleSizes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchFirstBarPullbacks_BaseTrades_Id",
                table: "ResearchFirstBarPullbacks",
                column: "Id",
                principalTable: "BaseTrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseTrades_SampleSizes_SampleSizeId",
                table: "BaseTrades");

            migrationBuilder.DropForeignKey(
                name: "FK_ResearchFirstBarPullbacks_BaseTrades_Id",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.AddColumn<int>(
                name: "TradeType",
                table: "BaseTrades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseTrades_SampleSizes_SampleSizeId",
                table: "BaseTrades",
                column: "SampleSizeId",
                principalTable: "SampleSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResearchFirstBarPullbacks_BaseTrades_Id",
                table: "ResearchFirstBarPullbacks",
                column: "Id",
                principalTable: "BaseTrades",
                principalColumn: "Id");
        }
    }
}
