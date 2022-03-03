using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Add_BedroomCcount_field_to_Survey_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_CreatedBy",
                table: "Users");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SurveyDate",
                table: "Surveys",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<int>(
                name: "BedroomCount",
                table: "Surveys",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy",
                unique: true,
                filter: "[CreatedBy] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_UpdatedBy",
                table: "Surveys",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_Users_UpdatedBy",
                table: "Surveys",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Users_UpdatedBy",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Surveys_UpdatedBy",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "BedroomCount",
                table: "Surveys");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SurveyDate",
                table: "Surveys",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy");
        }
    }
}
