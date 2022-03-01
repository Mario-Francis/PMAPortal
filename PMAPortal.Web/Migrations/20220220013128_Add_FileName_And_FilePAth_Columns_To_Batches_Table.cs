using Microsoft.EntityFrameworkCore.Migrations;

namespace PMAPortal.Web.Migrations
{
    public partial class Add_FileName_And_FilePAth_Columns_To_Batches_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Batches_BatchId",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Batches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Batches",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Batches_BatchId",
                table: "Customers",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Batches_BatchId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Batches");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Batches_BatchId",
                table: "Customers",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id");
        }
    }
}
