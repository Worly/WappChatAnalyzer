using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WappChatAnalyzer.Migrations
{
    public partial class AddImportHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ImportDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FirstMessageDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastMessageDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MessageCount = table.Column<int>(type: "int", nullable: false),
                    FromMessageId = table.Column<int>(type: "int", nullable: false),
                    ToMessageId = table.Column<int>(type: "int", nullable: false),
                    Overlap = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportHistories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportHistories");
        }
    }
}
