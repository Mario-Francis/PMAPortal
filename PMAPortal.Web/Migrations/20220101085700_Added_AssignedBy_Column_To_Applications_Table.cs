using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Added_AssignedBy_Column_To_Applications_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_InstallerId",
                table: "Applications");

            migrationBuilder.AddColumn<long>(
                name: "AssignedBy",
                table: "Applications",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AssignedByUserId",
                table: "Applications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_AssignedByUserId",
                table: "Applications",
                column: "AssignedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_AssignedByUserId",
                table: "Applications",
                column: "AssignedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_InstallerId",
                table: "Applications",
                column: "InstallerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_AssignedByUserId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_InstallerId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_AssignedByUserId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "AssignedBy",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "AssignedByUserId",
                table: "Applications");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_InstallerId",
                table: "Applications",
                column: "InstallerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
