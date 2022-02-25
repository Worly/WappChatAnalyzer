using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WappChatAnalyzer.Migrations
{
    public partial class RevertChangeMessagesIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_SentDate_SentTime_Id",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SentDate_SentTime",
                table: "Messages",
                columns: new[] { "SentDate", "SentTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_SentDate_SentTime",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SentDate_SentTime_Id",
                table: "Messages",
                columns: new[] { "SentDate", "SentTime", "Id" });
        }
    }
}
