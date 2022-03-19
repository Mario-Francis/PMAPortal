using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Add_principal_key_in_customer_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstallationBatchItems_Customers_CustomerAccountNumber",
                table: "InstallationBatchItems");

            migrationBuilder.DropIndex(
                name: "IX_InstallationBatchItems_CustomerAccountNumber",
                table: "InstallationBatchItems");

            migrationBuilder.DropColumn(
                name: "CustomerAccountNumber",
                table: "InstallationBatchItems");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "InstallationBatchItems",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstallationBatchItems_AccountNumber",
                table: "InstallationBatchItems",
                column: "AccountNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_InstallationBatchItems_Customers_AccountNumber",
                table: "InstallationBatchItems",
                column: "AccountNumber",
                principalTable: "Customers",
                principalColumn: "AccountNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstallationBatchItems_Customers_AccountNumber",
                table: "InstallationBatchItems");

            migrationBuilder.DropIndex(
                name: "IX_InstallationBatchItems_AccountNumber",
                table: "InstallationBatchItems");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "InstallationBatchItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerAccountNumber",
                table: "InstallationBatchItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstallationBatchItems_CustomerAccountNumber",
                table: "InstallationBatchItems",
                column: "CustomerAccountNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_InstallationBatchItems_Customers_CustomerAccountNumber",
                table: "InstallationBatchItems",
                column: "CustomerAccountNumber",
                principalTable: "Customers",
                principalColumn: "AccountNumber");
        }
    }
}
