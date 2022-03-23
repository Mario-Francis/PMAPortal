using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Modify_Feedback_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantFeedbacks_Applicants_ApplicantId",
                table: "ApplicantFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantFeedbacks_Applications_ApplicationId",
                table: "ApplicantFeedbacks");

            migrationBuilder.AlterColumn<long>(
                name: "ApplicationId",
                table: "ApplicantFeedbacks",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ApplicantId",
                table: "ApplicantFeedbacks",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "ApplicantFeedbacks",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "InstallationId",
                table: "ApplicantFeedbacks",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantFeedbacks_CustomerId",
                table: "ApplicantFeedbacks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantFeedbacks_InstallationId",
                table: "ApplicantFeedbacks",
                column: "InstallationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantFeedbacks_Applicants_ApplicantId",
                table: "ApplicantFeedbacks",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantFeedbacks_Applications_ApplicationId",
                table: "ApplicantFeedbacks",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantFeedbacks_Customers_CustomerId",
                table: "ApplicantFeedbacks",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantFeedbacks_Installations_InstallationId",
                table: "ApplicantFeedbacks",
                column: "InstallationId",
                principalTable: "Installations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantFeedbacks_Applicants_ApplicantId",
                table: "ApplicantFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantFeedbacks_Applications_ApplicationId",
                table: "ApplicantFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantFeedbacks_Customers_CustomerId",
                table: "ApplicantFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantFeedbacks_Installations_InstallationId",
                table: "ApplicantFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantFeedbacks_CustomerId",
                table: "ApplicantFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantFeedbacks_InstallationId",
                table: "ApplicantFeedbacks");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ApplicantFeedbacks");

            migrationBuilder.DropColumn(
                name: "InstallationId",
                table: "ApplicantFeedbacks");

            migrationBuilder.AlterColumn<long>(
                name: "ApplicationId",
                table: "ApplicantFeedbacks",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ApplicantId",
                table: "ApplicantFeedbacks",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantFeedbacks_Applicants_ApplicantId",
                table: "ApplicantFeedbacks",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantFeedbacks_Applications_ApplicationId",
                table: "ApplicantFeedbacks",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
