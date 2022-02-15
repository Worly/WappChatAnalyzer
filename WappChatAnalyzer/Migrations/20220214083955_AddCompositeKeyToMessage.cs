using Microsoft.EntityFrameworkCore.Migrations;

namespace WappChatAnalyzer.Migrations
{
    public partial class AddCompositeKeyToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                columns: new[] { "Id", "WorkspaceId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");
        }
    }
}
