using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDecimalToTriggerPrice5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TriggerPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsLoss",
                table: "ResearchFirstBarPullbacks",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TriggerPrice",
                table: "PaperTrades",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsLoss",
                table: "PaperTrades",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TriggerPrice",
                table: "ResearchFirstBarPullbacks",
                type: "decimal(20,8)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsLoss",
                table: "ResearchFirstBarPullbacks",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<decimal>(
                name: "TriggerPrice",
                table: "PaperTrades",
                type: "decimal(20,8)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsLoss",
                table: "PaperTrades",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
