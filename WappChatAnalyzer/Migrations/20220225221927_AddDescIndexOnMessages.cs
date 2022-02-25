using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WappChatAnalyzer.Migrations
{
    public partial class AddDescIndexOnMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE INDEX IX_Messages_SentDate_DESC_SentTime ON messages(SentDate DESC, SentTime ASC)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IX_Messages_SentDate_DESC_SentTime");
        }
    }
}
