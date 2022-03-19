using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Add_models_for_InstallationBatch_InstallationBatchItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_AccountNumber",
                table: "Customers");

            migrationBuilder.AddColumn<long>(
                name: "SurveyId",
                table: "Installations",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "Customers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Customers_AccountNumber",
                table: "Customers",
                column: "AccountNumber");

            migrationBuilder.CreateTable(
                name: "InstallationBatches",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    DateShared = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallationBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstallationBatches_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstallationBatchItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    InstallationBatchId = table.Column<long>(nullable: false),
                    SN = table.Column<string>(nullable: true),
                    DateShared = table.Column<DateTimeOffset>(nullable: true),
                    BatchNumber = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    CustomerAccountNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallationBatchItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstallationBatchItems_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstallationBatchItems_Customers_CustomerAccountNumber",
                        column: x => x.CustomerAccountNumber,
                        principalTable: "Customers",
                        principalColumn: "AccountNumber");
                    table.ForeignKey(
                        name: "FK_InstallationBatchItems_InstallationBatches_InstallationBatchId",
                        column: x => x.InstallationBatchId,
                        principalTable: "InstallationBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Installations_SurveyId",
                table: "Installations",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AccountNumber",
                table: "Customers",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstallationBatches_CreatedBy",
                table: "InstallationBatches",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InstallationBatchItems_CreatedBy",
                table: "InstallationBatchItems",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InstallationBatchItems_CustomerAccountNumber",
                table: "InstallationBatchItems",
                column: "CustomerAccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_InstallationBatchItems_InstallationBatchId",
                table: "InstallationBatchItems",
                column: "InstallationBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Installations_Surveys_SurveyId",
                table: "Installations",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Installations_Surveys_SurveyId",
                table: "Installations");

            migrationBuilder.DropTable(
                name: "InstallationBatchItems");

            migrationBuilder.DropTable(
                name: "InstallationBatches");

            migrationBuilder.DropIndex(
                name: "IX_Installations_SurveyId",
                table: "Installations");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Customers_AccountNumber",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_AccountNumber",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SurveyId",
                table: "Installations");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AccountNumber",
                table: "Customers",
                column: "AccountNumber",
                unique: true,
                filter: "[AccountNumber] IS NOT NULL");
        }
    }
}
