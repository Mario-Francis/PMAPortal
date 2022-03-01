using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Add_NaviagtionPropert_For_CreatedBy_In_All_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_CreatedBy",
                table: "UserRoles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_CreatedBy",
                table: "Surveys",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedBy",
                table: "Roles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_CreatedBy",
                table: "Pets",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreatedBy",
                table: "Payments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLogs_CreatedBy",
                table: "PaymentLogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Meters_CreatedBy",
                table: "Meters",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Mails_CreatedBy",
                table: "Mails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InstallationStatuses_CreatedBy",
                table: "InstallationStatuses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Installations_CreatedBy",
                table: "Installations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HouseTypes_CreatedBy",
                table: "HouseTypes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackQuestions_CreatedBy",
                table: "FeedbackQuestions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackAnswers_CreatedBy",
                table: "FeedbackAnswers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerStatusLogs_CreatedBy",
                table: "CustomerStatusLogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedBy",
                table: "Customers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_CreatedBy",
                table: "Batches",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedBy",
                table: "AuditLogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogChanges_CreatedBy",
                table: "AuditLogChanges",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_CreatedBy",
                table: "Areas",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStatusLogs_CreatedBy",
                table: "ApplicationStatusLogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStatuses_CreatedBy",
                table: "ApplicationStatuses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CreatedBy",
                table: "Applications",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationPets_CreatedBy",
                table: "ApplicationPets",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationAppliances_CreatedBy",
                table: "ApplicationAppliances",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_CreatedBy",
                table: "Applicants",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantFeedbacks_CreatedBy",
                table: "ApplicantFeedbacks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAddresses_CreatedBy",
                table: "ApplicantAddresses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Appliances_CreatedBy",
                table: "Appliances",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CreatedBy",
                table: "ActivityLogs",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_Users_CreatedBy",
                table: "ActivityLogs",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appliances_Users_CreatedBy",
                table: "Appliances",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantAddresses_Users_CreatedBy",
                table: "ApplicantAddresses",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantFeedbacks_Users_CreatedBy",
                table: "ApplicantFeedbacks",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Users_CreatedBy",
                table: "Applicants",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationAppliances_Users_CreatedBy",
                table: "ApplicationAppliances",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationPets_Users_CreatedBy",
                table: "ApplicationPets",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_CreatedBy",
                table: "Applications",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationStatuses_Users_CreatedBy",
                table: "ApplicationStatuses",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationStatusLogs_Users_CreatedBy",
                table: "ApplicationStatusLogs",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Users_CreatedBy",
                table: "Areas",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogChanges_Users_CreatedBy",
                table: "AuditLogChanges",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_CreatedBy",
                table: "AuditLogs",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_Users_CreatedBy",
                table: "Batches",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_CreatedBy",
                table: "Customers",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerStatusLogs_Users_CreatedBy",
                table: "CustomerStatusLogs",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackAnswers_Users_CreatedBy",
                table: "FeedbackAnswers",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackQuestions_Users_CreatedBy",
                table: "FeedbackQuestions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HouseTypes_Users_CreatedBy",
                table: "HouseTypes",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Installations_Users_CreatedBy",
                table: "Installations",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InstallationStatuses_Users_CreatedBy",
                table: "InstallationStatuses",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mails_Users_CreatedBy",
                table: "Mails",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meters_Users_CreatedBy",
                table: "Meters",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentLogs_Users_CreatedBy",
                table: "PaymentLogs",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_CreatedBy",
                table: "Payments",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Users_CreatedBy",
                table: "Pets",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_CreatedBy",
                table: "Roles",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_Users_CreatedBy",
                table: "Surveys",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_CreatedBy",
                table: "UserRoles",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_Users_CreatedBy",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Appliances_Users_CreatedBy",
                table: "Appliances");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantAddresses_Users_CreatedBy",
                table: "ApplicantAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantFeedbacks_Users_CreatedBy",
                table: "ApplicantFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Users_CreatedBy",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationAppliances_Users_CreatedBy",
                table: "ApplicationAppliances");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationPets_Users_CreatedBy",
                table: "ApplicationPets");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_CreatedBy",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationStatuses_Users_CreatedBy",
                table: "ApplicationStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationStatusLogs_Users_CreatedBy",
                table: "ApplicationStatusLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Users_CreatedBy",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogChanges_Users_CreatedBy",
                table: "AuditLogChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_CreatedBy",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Batches_Users_CreatedBy",
                table: "Batches");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_CreatedBy",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerStatusLogs_Users_CreatedBy",
                table: "CustomerStatusLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackAnswers_Users_CreatedBy",
                table: "FeedbackAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackQuestions_Users_CreatedBy",
                table: "FeedbackQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_HouseTypes_Users_CreatedBy",
                table: "HouseTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Installations_Users_CreatedBy",
                table: "Installations");

            migrationBuilder.DropForeignKey(
                name: "FK_InstallationStatuses_Users_CreatedBy",
                table: "InstallationStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Mails_Users_CreatedBy",
                table: "Mails");

            migrationBuilder.DropForeignKey(
                name: "FK_Meters_Users_CreatedBy",
                table: "Meters");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentLogs_Users_CreatedBy",
                table: "PaymentLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_CreatedBy",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Users_CreatedBy",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_CreatedBy",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Users_CreatedBy",
                table: "Surveys");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_CreatedBy",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_CreatedBy",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Surveys_CreatedBy",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_Roles_CreatedBy",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Pets_CreatedBy",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CreatedBy",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_PaymentLogs_CreatedBy",
                table: "PaymentLogs");

            migrationBuilder.DropIndex(
                name: "IX_Meters_CreatedBy",
                table: "Meters");

            migrationBuilder.DropIndex(
                name: "IX_Mails_CreatedBy",
                table: "Mails");

            migrationBuilder.DropIndex(
                name: "IX_InstallationStatuses_CreatedBy",
                table: "InstallationStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Installations_CreatedBy",
                table: "Installations");

            migrationBuilder.DropIndex(
                name: "IX_HouseTypes_CreatedBy",
                table: "HouseTypes");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackQuestions_CreatedBy",
                table: "FeedbackQuestions");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackAnswers_CreatedBy",
                table: "FeedbackAnswers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerStatusLogs_CreatedBy",
                table: "CustomerStatusLogs");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CreatedBy",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Batches_CreatedBy",
                table: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_CreatedBy",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogChanges_CreatedBy",
                table: "AuditLogChanges");

            migrationBuilder.DropIndex(
                name: "IX_Areas_CreatedBy",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationStatusLogs_CreatedBy",
                table: "ApplicationStatusLogs");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationStatuses_CreatedBy",
                table: "ApplicationStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Applications_CreatedBy",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationPets_CreatedBy",
                table: "ApplicationPets");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationAppliances_CreatedBy",
                table: "ApplicationAppliances");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_CreatedBy",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantFeedbacks_CreatedBy",
                table: "ApplicantFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantAddresses_CreatedBy",
                table: "ApplicantAddresses");

            migrationBuilder.DropIndex(
                name: "IX_Appliances_CreatedBy",
                table: "Appliances");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_CreatedBy",
                table: "ActivityLogs");
        }
    }
}
