using Microsoft.EntityFrameworkCore.Migrations;

namespace WappChatAnalyzer.Migrations
{
    public partial class UpdateStatisticCacheUniqueConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Name_Date",
                table: "StatisticCaches");

            migrationBuilder.CreateIndex(
                name: "IX_Name_Date_Workspace",
                table: "StatisticCaches",
                columns: new[] { "StatisticName", "ForDate", "WorkspaceId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Name_Date_Workspace",
                table: "StatisticCaches");

            migrationBuilder.CreateIndex(
                name: "IX_Name_Date",
                table: "StatisticCaches",
                columns: new[] { "StatisticName", "ForDate" },
                unique: true);
        }
    }
}
