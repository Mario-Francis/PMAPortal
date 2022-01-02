using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Added_InnstallerId_Column_In_Applications_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InstallerId",
                table: "Applications",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "Supervisor");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Name",
                value: "Disco Personnel");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Name" },
                values: new object[] { 4L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Installer" });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_InstallerId",
                table: "Applications",
                column: "InstallerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_InstallerId",
                table: "Applications",
                column: "InstallerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_InstallerId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_InstallerId",
                table: "Applications");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DropColumn(
                name: "InstallerId",
                table: "Applications");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "Disco Personnel");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Name",
                value: "Installer");
        }
    }
}
