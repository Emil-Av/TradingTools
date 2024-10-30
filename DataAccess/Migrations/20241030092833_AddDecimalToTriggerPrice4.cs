using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDecimalToTriggerPrice4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TriggerPrice",
                table: "ResearchFirstBarPullbacks",
                type: "decimal(10,10)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TriggerPrice",
                table: "PaperTrades",
                type: "decimal(10,10)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TriggerPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TriggerPrice",
                table: "PaperTrades",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,10)",
                oldNullable: true);
        }
    }
}
