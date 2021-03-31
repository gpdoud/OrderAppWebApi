using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderAppWebApi.Migrations
{
    public partial class AddedSalesperson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalespersonId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Salespeople",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    StateCode = table.Column<string>(nullable: true),
                    Sales = table.Column<decimal>(type: "decimal(9,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salespeople", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SalespersonId",
                table: "Orders",
                column: "SalespersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Salespeople_SalespersonId",
                table: "Orders",
                column: "SalespersonId",
                principalTable: "Salespeople",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Salespeople_SalespersonId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Salespeople");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SalespersonId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SalespersonId",
                table: "Orders");
        }
    }
}
