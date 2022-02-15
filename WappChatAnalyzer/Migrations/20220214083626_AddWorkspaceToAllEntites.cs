using Microsoft.EntityFrameworkCore.Migrations;

namespace WappChatAnalyzer.Migrations
{
    public partial class AddWorkspaceToAllEntites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "StatisticCaches",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "Senders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "ImportHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "CustomStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomStatistics_Workspaces_WorkspaceId",
                table: "CustomStatistics",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Workspaces_WorkspaceId",
                table: "Events",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportHistories_Workspaces_WorkspaceId",
                table: "ImportHistories",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Workspaces_WorkspaceId",
                table: "Messages",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Senders_Workspaces_WorkspaceId",
                table: "Senders",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StatisticCaches_Workspaces_WorkspaceId",
                table: "StatisticCaches",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "StatisticCaches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "Senders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "Messages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "ImportHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "Events",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceId",
                table: "CustomStatistics",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}
