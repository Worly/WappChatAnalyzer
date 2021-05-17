using Microsoft.EntityFrameworkCore.Migrations;

namespace WappChatAnalyzer.Migrations
{
    public partial class AddOrderToEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "UPDATE events AS e " +
                "JOIN(SELECT Id, ROW_NUMBER() OVER(PARTITION BY DateTime ORDER BY Id) AS rn FROM events) AS sub ON e.Id = sub.Id " +
                "SET e.Order = sub.rn ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Events");
        }
    }
}
