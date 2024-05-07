using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaperTrades_SampleSize_SampleSizeId",
                table: "PaperTrades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SampleSize",
                table: "SampleSize");

            migrationBuilder.RenameTable(
                name: "SampleSize",
                newName: "SamplesSizes");

            migrationBuilder.AlterColumn<int>(
                name: "TimeFrame",
                table: "SamplesSizes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Strategy",
                table: "SamplesSizes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SamplesSizes",
                table: "SamplesSizes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PTTimeFrame = table.Column<int>(type: "int", nullable: false),
                    PTStrategy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PaperTrades_SamplesSizes_SampleSizeId",
                table: "PaperTrades",
                column: "SampleSizeId",
                principalTable: "SamplesSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaperTrades_SamplesSizes_SampleSizeId",
                table: "PaperTrades");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SamplesSizes",
                table: "SamplesSizes");

            migrationBuilder.RenameTable(
                name: "SamplesSizes",
                newName: "SampleSize");

            migrationBuilder.AlterColumn<int>(
                name: "TimeFrame",
                table: "SampleSize",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Strategy",
                table: "SampleSize",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SampleSize",
                table: "SampleSize",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaperTrades_SampleSize_SampleSizeId",
                table: "PaperTrades",
                column: "SampleSizeId",
                principalTable: "SampleSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
