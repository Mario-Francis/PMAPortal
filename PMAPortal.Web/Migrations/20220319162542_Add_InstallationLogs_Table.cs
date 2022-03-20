using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Add_InstallationLogs_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstallationLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    InstallationId = table.Column<long>(nullable: false),
                    ActionBy = table.Column<long>(nullable: true),
                    InstallationStatusId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    InstallationId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstallationLogs_Users_ActionBy",
                        column: x => x.ActionBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InstallationLogs_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstallationLogs_Installations_InstallationId",
                        column: x => x.InstallationId,
                        principalTable: "Installations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InstallationLogs_Installations_InstallationId1",
                        column: x => x.InstallationId1,
                        principalTable: "Installations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstallationLogs_InstallationStatuses_InstallationStatusId",
                        column: x => x.InstallationStatusId,
                        principalTable: "InstallationStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstallationLogs_ActionBy",
                table: "InstallationLogs",
                column: "ActionBy");

            migrationBuilder.CreateIndex(
                name: "IX_InstallationLogs_CreatedBy",
                table: "InstallationLogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InstallationLogs_InstallationId",
                table: "InstallationLogs",
                column: "InstallationId");

            migrationBuilder.CreateIndex(
                name: "IX_InstallationLogs_InstallationId1",
                table: "InstallationLogs",
                column: "InstallationId1");

            migrationBuilder.CreateIndex(
                name: "IX_InstallationLogs_InstallationStatusId",
                table: "InstallationLogs",
                column: "InstallationStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstallationLogs");
        }
    }
}
