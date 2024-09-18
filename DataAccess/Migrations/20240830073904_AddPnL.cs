using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPnL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Loss",
                table: "ResearchFirstBarPullbacks");

            migrationBuilder.DropColumn(
                name: "Loss",
                table: "PaperTrades");

            migrationBuilder.RenameColumn(
                name: "Profit",
                table: "ResearchFirstBarPullbacks",
                newName: "PnL");

            migrationBuilder.RenameColumn(
                name: "Profit",
                table: "PaperTrades",
                newName: "PnL");

            migrationBuilder.AddColumn<bool>(
                name: "IsLoss",
                table: "PaperTrades",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLoss",
                table: "PaperTrades");

            migrationBuilder.RenameColumn(
                name: "PnL",
                table: "ResearchFirstBarPullbacks",
                newName: "Profit");

            migrationBuilder.RenameColumn(
                name: "PnL",
                table: "PaperTrades",
                newName: "Profit");

            migrationBuilder.AddColumn<double>(
                name: "Loss",
                table: "ResearchFirstBarPullbacks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Loss",
                table: "PaperTrades",
                type: "float",
                nullable: true);
        }
    }
}
