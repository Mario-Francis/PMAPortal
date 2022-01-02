using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Added_FeedbackQuestions_And_Answers_Table2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackAnswer_ApplicantFeedbacks_ApplicantFeedbackId",
                table: "FeedbackAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackAnswer_FeedbackQuestion_FeedbackQuestionId",
                table: "FeedbackAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedbackQuestion",
                table: "FeedbackQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedbackAnswer",
                table: "FeedbackAnswer");

            migrationBuilder.RenameTable(
                name: "FeedbackQuestion",
                newName: "FeedbackQuestions");

            migrationBuilder.RenameTable(
                name: "FeedbackAnswer",
                newName: "FeedbackAnswers");

            migrationBuilder.RenameIndex(
                name: "IX_FeedbackAnswer_FeedbackQuestionId",
                table: "FeedbackAnswers",
                newName: "IX_FeedbackAnswers_FeedbackQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedbackAnswer_ApplicantFeedbackId",
                table: "FeedbackAnswers",
                newName: "IX_FeedbackAnswers_ApplicantFeedbackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedbackQuestions",
                table: "FeedbackQuestions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedbackAnswers",
                table: "FeedbackAnswers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackAnswers_ApplicantFeedbacks_ApplicantFeedbackId",
                table: "FeedbackAnswers",
                column: "ApplicantFeedbackId",
                principalTable: "ApplicantFeedbacks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackAnswers_FeedbackQuestions_FeedbackQuestionId",
                table: "FeedbackAnswers",
                column: "FeedbackQuestionId",
                principalTable: "FeedbackQuestions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackAnswers_ApplicantFeedbacks_ApplicantFeedbackId",
                table: "FeedbackAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackAnswers_FeedbackQuestions_FeedbackQuestionId",
                table: "FeedbackAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedbackQuestions",
                table: "FeedbackQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedbackAnswers",
                table: "FeedbackAnswers");

            migrationBuilder.RenameTable(
                name: "FeedbackQuestions",
                newName: "FeedbackQuestion");

            migrationBuilder.RenameTable(
                name: "FeedbackAnswers",
                newName: "FeedbackAnswer");

            migrationBuilder.RenameIndex(
                name: "IX_FeedbackAnswers_FeedbackQuestionId",
                table: "FeedbackAnswer",
                newName: "IX_FeedbackAnswer_FeedbackQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedbackAnswers_ApplicantFeedbackId",
                table: "FeedbackAnswer",
                newName: "IX_FeedbackAnswer_ApplicantFeedbackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedbackQuestion",
                table: "FeedbackQuestion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedbackAnswer",
                table: "FeedbackAnswer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackAnswer_ApplicantFeedbacks_ApplicantFeedbackId",
                table: "FeedbackAnswer",
                column: "ApplicantFeedbackId",
                principalTable: "ApplicantFeedbacks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackAnswer_FeedbackQuestion_FeedbackQuestionId",
                table: "FeedbackAnswer",
                column: "FeedbackQuestionId",
                principalTable: "FeedbackQuestion",
                principalColumn: "Id");
        }
    }
}
