using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDecimalToTriggerPrice5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TriggerPrice",
                table: "ResearchFirstBarPullbacks",
                type: "decimal(20,8)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TriggerPrice",
                table: "PaperTrades",
                type: "decimal(20,8)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,10)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TriggerPrice",
                table: "ResearchFirstBarPullbacks",
                type: "decimal(10,10)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TriggerPrice",
                table: "PaperTrades",
                type: "decimal(10,10)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)",
                oldNullable: true);
        }
    }
}
