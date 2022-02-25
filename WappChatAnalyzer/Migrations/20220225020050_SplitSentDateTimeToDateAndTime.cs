using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WappChatAnalyzer.Migrations
{
    public partial class SplitSentDateTimeToDateAndTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "SentDate",
                table: "Messages",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "SentTime",
                table: "Messages",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SentDate_SentTime",
                table: "Messages",
                columns: new[] { "SentDate", "SentTime" });

            migrationBuilder.Sql("UPDATE Messages SET SentDate = DATE(SentDateTime), SentTime = TIME(SentDateTime)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_SentDate_SentTime",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SentDate",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SentTime",
                table: "Messages");
        }
    }
}
