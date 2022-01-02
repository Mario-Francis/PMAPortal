using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Added_FeedbackQuestions_And_Answers_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedbackQuestion",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    Question = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackQuestion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackAnswer",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ApplicantFeedbackId = table.Column<long>(nullable: false),
                    FeedbackQuestionId = table.Column<long>(nullable: false),
                    rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbackAnswer_ApplicantFeedbacks_ApplicantFeedbackId",
                        column: x => x.ApplicantFeedbackId,
                        principalTable: "ApplicantFeedbacks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FeedbackAnswer_FeedbackQuestion_FeedbackQuestionId",
                        column: x => x.FeedbackQuestionId,
                        principalTable: "FeedbackQuestion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantFeedbacks_ApplicationId",
                table: "ApplicantFeedbacks",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackAnswer_ApplicantFeedbackId",
                table: "FeedbackAnswer",
                column: "ApplicantFeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackAnswer_FeedbackQuestionId",
                table: "FeedbackAnswer",
                column: "FeedbackQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantFeedbacks_Applications_ApplicationId",
                table: "ApplicantFeedbacks",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantFeedbacks_Applications_ApplicationId",
                table: "ApplicantFeedbacks");

            migrationBuilder.DropTable(
                name: "FeedbackAnswer");

            migrationBuilder.DropTable(
                name: "FeedbackQuestion");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantFeedbacks_ApplicationId",
                table: "ApplicantFeedbacks");
        }
    }
}
