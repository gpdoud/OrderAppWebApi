using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderAppWebApi.Migrations
{
    public partial class Madesalespersonarequiredvalueinorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Salespeople_SalespersonId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "SalespersonId",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Salespeople_SalespersonId",
                table: "Orders",
                column: "SalespersonId",
                principalTable: "Salespeople",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Salespeople_SalespersonId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "SalespersonId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Salespeople_SalespersonId",
                table: "Orders",
                column: "SalespersonId",
                principalTable: "Salespeople",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
