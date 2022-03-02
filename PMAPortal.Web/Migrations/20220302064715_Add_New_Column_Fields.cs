using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Add_New_Column_Fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BU",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "DT",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "Feeder",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "MeteredStatus",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "Tariff",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "UT",
                table: "Surveys");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ScheduleDate",
                table: "Surveys",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ScheduleDate",
                table: "Installations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BU",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BatchNumber",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DT",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateShared",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feeder",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeteredStatus",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SN",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tariff",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UT",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ScheduleDate",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "ScheduleDate",
                table: "Installations");

            migrationBuilder.DropColumn(
                name: "BU",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BatchNumber",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DT",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DateShared",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Feeder",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "MeteredStatus",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SN",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Tariff",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UT",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "BU",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DT",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feeder",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeteredStatus",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tariff",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UT",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
