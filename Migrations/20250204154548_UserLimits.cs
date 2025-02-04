using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceControl.Migrations
{
    /// <inheritdoc />
    public partial class UserLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AnnualLimit",
                table: "Users",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyLimit",
                table: "Users",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyLimit",
                table: "Users",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeeklyLimit",
                table: "Users",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualLimit",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DailyLimit",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MonthlyLimit",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WeeklyLimit",
                table: "Users");
        }
    }
}
