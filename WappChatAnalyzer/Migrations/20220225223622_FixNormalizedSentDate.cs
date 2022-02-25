using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WappChatAnalyzer.Migrations
{
    public partial class FixNormalizedSentDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "NormalizedSentDate",
                table: "Messages",
                type: "date",
                nullable: false,
                computedColumnSql: "CASE WHEN EXTRACT(hour FROM SentTime) < 7 THEN DATE_ADD(SentDate, INTERVAL -1 day) ELSE SentDate END",
                stored: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedSentDate",
                table: "Messages");
        }
    }
}
