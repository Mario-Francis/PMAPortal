using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Move_image_fields_in_survey_table_to_installation_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerBillImagePath",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "LocationFrontViewImagePath",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "MeterPointBeforeInstallationImagePath",
                table: "Surveys");

            migrationBuilder.AddColumn<string>(
                name: "CustomerBillImagePath",
                table: "Installations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationFrontViewImagePath",
                table: "Installations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeterPointBeforeInstallationImagePath",
                table: "Installations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerBillImagePath",
                table: "Installations");

            migrationBuilder.DropColumn(
                name: "LocationFrontViewImagePath",
                table: "Installations");

            migrationBuilder.DropColumn(
                name: "MeterPointBeforeInstallationImagePath",
                table: "Installations");

            migrationBuilder.AddColumn<string>(
                name: "CustomerBillImagePath",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationFrontViewImagePath",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeterPointBeforeInstallationImagePath",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
