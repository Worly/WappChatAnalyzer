using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WappChatAnalyzer.Migrations
{
    public partial class RevertMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_Id",
                table: "Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Messages_Id",
                table: "Messages",
                column: "Id");
        }
    }
}
