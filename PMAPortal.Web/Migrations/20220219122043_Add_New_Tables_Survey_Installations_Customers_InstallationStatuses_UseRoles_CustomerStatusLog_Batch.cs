using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Add_New_Tables_Survey_Installations_Customers_InstallationStatuses_UseRoles_CustomerStatusLog_Batch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_InstallerId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    DateShared = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstallationStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    BatchId = table.Column<long>(nullable: false),
                    AccountNumber = table.Column<string>(nullable: true),
                    ARN = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    CISName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    CISAddress = table.Column<string>(nullable: true),
                    Landmark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerStatusLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false),
                    ActionBy = table.Column<long>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    InstallationStatusId = table.Column<long>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerStatusLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerStatusLogs_Users_ActionBy",
                        column: x => x.ActionBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerStatusLogs_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerStatusLogs_InstallationStatuses_InstallationStatusId",
                        column: x => x.InstallationStatusId,
                        principalTable: "InstallationStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Installations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false),
                    InstallationStatusId = table.Column<long>(nullable: false),
                    InstallerId = table.Column<long>(nullable: true),
                    AssignedBy = table.Column<long>(nullable: true),
                    MeterType = table.Column<string>(nullable: true),
                    MeterNumber = table.Column<string>(nullable: true),
                    MeterNamePlateImagePath = table.Column<string>(nullable: true),
                    InProgessImagePath = table.Column<string>(nullable: true),
                    MeterSealImagePath = table.Column<string>(nullable: true),
                    CompleteWithSealImagePath = table.Column<string>(nullable: true),
                    MeterCardImagePath = table.Column<string>(nullable: true),
                    SupplyCableVisibleToMeterImagePath = table.Column<string>(nullable: true),
                    SupplyCableToThePremisesImagePath = table.Column<string>(nullable: true),
                    RetrievedMeterImagePath = table.Column<string>(nullable: true),
                    MonitorShowingRemainingUnitImagePath = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Installations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Installations_Users_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Installations_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Installations_InstallationStatuses_InstallationStatusId",
                        column: x => x.InstallationStatusId,
                        principalTable: "InstallationStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Installations_Users_InstallerId",
                        column: x => x.InstallerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false),
                    BU = table.Column<string>(nullable: true),
                    UT = table.Column<string>(nullable: true),
                    Feeder = table.Column<string>(nullable: true),
                    DT = table.Column<string>(nullable: true),
                    Tariff = table.Column<string>(nullable: true),
                    MeteredStatus = table.Column<string>(nullable: true),
                    ReadyToPay = table.Column<string>(nullable: true),
                    OccupierPhoneNumber = table.Column<string>(nullable: true),
                    TypeOfApartment = table.Column<string>(nullable: true),
                    ExistingMeterType = table.Column<string>(nullable: true),
                    ExistingMeterNumber = table.Column<string>(nullable: true),
                    CustomerBillMatchUploadedData = table.Column<string>(nullable: true),
                    EstimatedTotalLoadInAmps = table.Column<decimal>(type: "decimal(8, 2)", nullable: true),
                    RecommendedMeterType = table.Column<string>(nullable: true),
                    InstallationMode = table.Column<string>(nullable: true),
                    LoadWireSeparationRequired = table.Column<string>(nullable: true),
                    AccountSeparationRequired = table.Column<string>(nullable: true),
                    NumberOf1QRequired = table.Column<int>(nullable: true),
                    NumberOf3QRequired = table.Column<int>(nullable: true),
                    SurveryCompany = table.Column<string>(nullable: true),
                    SurveyStaffId = table.Column<long>(nullable: true),
                    AssignedBy = table.Column<long>(nullable: true),
                    SurveyDate = table.Column<DateTimeOffset>(nullable: false),
                    SurveyRemark = table.Column<string>(nullable: true),
                    MAP = table.Column<string>(nullable: true),
                    AdditionalComment = table.Column<string>(nullable: true),
                    LocationFrontViewImagePath = table.Column<string>(nullable: true),
                    MeterPointBeforeInstallationImagePath = table.Column<string>(nullable: true),
                    CustomerBillImagePath = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<long>(nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surveys_Users_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Surveys_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Surveys_Users_SurveyStaffId",
                        column: x => x.SurveyStaffId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "InstallationStatuses",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Name" },
                values: new object[,]
                {
                    { 1L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Pending" },
                    { 2L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Scheduled for Installation" },
                    { 3L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Installation In Progress" },
                    { 4L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Installation Failed" },
                    { 5L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Installation Completed" },
                    { 6L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Disco Confirmation Failed" },
                    { 7L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Disco Confirmation Successful" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Name" },
                values: new object[] { 5L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Survey Staff" });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AccountNumber",
                table: "Customers",
                column: "AccountNumber",
                unique: true,
                filter: "[AccountNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BatchId",
                table: "Customers",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerStatusLogs_ActionBy",
                table: "CustomerStatusLogs",
                column: "ActionBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerStatusLogs_CustomerId",
                table: "CustomerStatusLogs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerStatusLogs_InstallationStatusId",
                table: "CustomerStatusLogs",
                column: "InstallationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Installations_AssignedBy",
                table: "Installations",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Installations_CustomerId",
                table: "Installations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Installations_InstallationStatusId",
                table: "Installations",
                column: "InstallationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Installations_InstallerId",
                table: "Installations",
                column: "InstallerId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_AssignedBy",
                table: "Surveys",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_CustomerId",
                table: "Surveys",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_SurveyStaffId",
                table: "Surveys",
                column: "SurveyStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_InstallerId",
                table: "Applications",
                column: "InstallerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_InstallerId",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "CustomerStatusLogs");

            migrationBuilder.DropTable(
                name: "Installations");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "InstallationStatuses");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.AddColumn<long>(
                name: "RoleId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "ApplicationStatuses",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Name" },
                values: new object[,]
                {
                    { 1L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Pending" },
                    { 2L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Submitted" },
                    { 3L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Scheduled for Installation" },
                    { 4L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Installation In Progress" },
                    { 5L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Installation Failed" },
                    { 6L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Installation Completed" },
                    { 7L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Disco Confirmation Failed" },
                    { 8L, null, new DateTimeOffset(new DateTime(2021, 10, 29, 18, 38, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Disco Confirmation Successful" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_InstallerId",
                table: "Applications",
                column: "InstallerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
