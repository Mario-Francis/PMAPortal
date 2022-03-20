using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Remove_duplicate_relationship_between_Installation_table_and_InstallationLogs_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstallationLogs_Installations_InstallationId",
                table: "InstallationLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_InstallationLogs_Installations_InstallationId1",
                table: "InstallationLogs");

            migrationBuilder.DropIndex(
                name: "IX_InstallationLogs_InstallationId1",
                table: "InstallationLogs");

            migrationBuilder.DropColumn(
                name: "InstallationId1",
                table: "InstallationLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_InstallationLogs_Installations_InstallationId",
                table: "InstallationLogs",
                column: "InstallationId",
                principalTable: "Installations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstallationLogs_Installations_InstallationId",
                table: "InstallationLogs");

            migrationBuilder.AddColumn<long>(
                name: "InstallationId1",
                table: "InstallationLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstallationLogs_InstallationId1",
                table: "InstallationLogs",
                column: "InstallationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_InstallationLogs_Installations_InstallationId",
                table: "InstallationLogs",
                column: "InstallationId",
                principalTable: "Installations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InstallationLogs_Installations_InstallationId1",
                table: "InstallationLogs",
                column: "InstallationId1",
                principalTable: "Installations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
