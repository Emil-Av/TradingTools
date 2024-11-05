using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Set0AsDefaultValues4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TriggerPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TradeDurationInCandles",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "StopPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PnL",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Fee",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ExitPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "EntryPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                defaultValueSql: "0",
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
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<int>(
                name: "TradeDurationInCandles",
                table: "ResearchFirstBarPullbacks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "StopPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "PnL",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "Fee",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "ExitPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<double>(
                name: "EntryPrice",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValueSql: "0");
        }
    }
}
