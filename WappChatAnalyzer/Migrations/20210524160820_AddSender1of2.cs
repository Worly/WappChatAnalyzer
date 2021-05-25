using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WappChatAnalyzer.Migrations
{
    public partial class AddSender1of2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sender",
                table: "StatisticCacheForSender");

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "StatisticCacheForSender",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Senders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Senders", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticCacheForSender_SenderId",
                table: "StatisticCacheForSender",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatisticCacheForSender_Senders_SenderId",
                table: "StatisticCacheForSender",
                column: "SenderId",
                principalTable: "Senders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql("INSERT INTO senders(name) " +
                "SELECT DISTINCT sender " +
                "FROM messages");
            migrationBuilder.Sql("UPDATE messages m " +
                "SET senderId = (SELECT id FROM senders WHERE name = m.sender)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatisticCacheForSender_Senders_SenderId",
                table: "StatisticCacheForSender");

            migrationBuilder.DropTable(
                name: "Senders");

            migrationBuilder.DropIndex(
                name: "IX_StatisticCacheForSender_SenderId",
                table: "StatisticCacheForSender");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "StatisticCacheForSender");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "StatisticCacheForSender",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
