using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WappChatAnalyzer.Migrations
{
    public partial class CompleteMigrateToPostgreSQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "NormalizedSentDate",
                table: "Messages",
                type: "date",
                nullable: false,
                computedColumnSql: "CASE WHEN EXTRACT(hour FROM \"SentTime\") < 7 THEN \"SentDate\" - INTERVAL '1 day' ELSE \"SentDate\" END",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_NormalizedSentDate",
                table: "Messages",
                column: "NormalizedSentDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_NormalizedSentDate",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "NormalizedSentDate",
                table: "Messages");
        }
    }
}
