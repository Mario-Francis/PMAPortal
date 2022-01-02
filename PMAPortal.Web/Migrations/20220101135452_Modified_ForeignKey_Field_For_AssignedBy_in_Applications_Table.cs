using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Modified_ForeignKey_Field_For_AssignedBy_in_Applications_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_AssignedByUserId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_AssignedByUserId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "AssignedByUserId",
                table: "Applications");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_AssignedBy",
                table: "Applications",
                column: "AssignedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_AssignedBy",
                table: "Applications",
                column: "AssignedBy",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_AssignedBy",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_AssignedBy",
                table: "Applications");

            migrationBuilder.AddColumn<long>(
                name: "AssignedByUserId",
                table: "Applications",
                type: "bigint",
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
        }
    }
}
