using Microsoft.EntityFrameworkCore.Migrations;

namespace WappChatAnalyzer.Migrations
{
    public partial class AddWorkspaceToAllEntites_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkspaceId",
                table: "StatisticCaches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkspaceId",
                table: "Senders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkspaceId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkspaceId",
                table: "ImportHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkspaceId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkspaceId",
                table: "CustomStatistics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatisticCaches_WorkspaceId",
                table: "StatisticCaches",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Senders_WorkspaceId",
                table: "Senders",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_WorkspaceId",
                table: "Messages",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportHistories_WorkspaceId",
                table: "ImportHistories",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_WorkspaceId",
                table: "Events",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomStatistics_WorkspaceId",
                table: "CustomStatistics",
                column: "WorkspaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomStatistics_Workspaces_WorkspaceId",
                table: "CustomStatistics",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Workspaces_WorkspaceId",
                table: "Events",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportHistories_Workspaces_WorkspaceId",
                table: "ImportHistories",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Workspaces_WorkspaceId",
                table: "Messages",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Senders_Workspaces_WorkspaceId",
                table: "Senders",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StatisticCaches_Workspaces_WorkspaceId",
                table: "StatisticCaches",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomStatistics_Workspaces_WorkspaceId",
                table: "CustomStatistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Workspaces_WorkspaceId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportHistories_Workspaces_WorkspaceId",
                table: "ImportHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Workspaces_WorkspaceId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Senders_Workspaces_WorkspaceId",
                table: "Senders");

            migrationBuilder.DropForeignKey(
                name: "FK_StatisticCaches_Workspaces_WorkspaceId",
                table: "StatisticCaches");

            migrationBuilder.DropIndex(
                name: "IX_StatisticCaches_WorkspaceId",
                table: "StatisticCaches");

            migrationBuilder.DropIndex(
                name: "IX_Senders_WorkspaceId",
                table: "Senders");

            migrationBuilder.DropIndex(
                name: "IX_Messages_WorkspaceId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_ImportHistories_WorkspaceId",
                table: "ImportHistories");

            migrationBuilder.DropIndex(
                name: "IX_Events_WorkspaceId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_CustomStatistics_WorkspaceId",
                table: "CustomStatistics");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "StatisticCaches");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Senders");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "ImportHistories");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "CustomStatistics");
        }
    }
}
