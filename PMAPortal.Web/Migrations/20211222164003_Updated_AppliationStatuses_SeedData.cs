using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Updated_AppliationStatuses_SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UpdatedBy",
                table: "Mails",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActionByType",
                table: "ActivityLogs",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Name",
                value: "Scheduled for Installation");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Name",
                value: "Installation In Progress");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Name",
                value: "Installation Failed");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Name",
                value: "Installation Completed");

            migrationBuilder.InsertData(
                table: "ApplicationStatuses",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Name" },
                values: new object[,]
                {
                    { 7L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Disco Confirmation Failed" },
                    { 8L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Disco Confirmation Successful" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DropColumn(
                name: "ActionByType",
                table: "ActivityLogs");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Mails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Name",
                value: "Installation Failed");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Name",
                value: "Installation Completed");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Name",
                value: "Disco Confirmation Failed");

            migrationBuilder.UpdateData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Name",
                value: "Disco Confirmation Successful");
        }
    }
}
