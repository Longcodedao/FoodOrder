using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOrder.Migrations
{
    public partial class UpdateOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderHeaderId",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_OrderHeaderId",
                table: "Carts",
                column: "OrderHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_OrderHeaders_OrderHeaderId",
                table: "Carts",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_OrderHeaders_OrderHeaderId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_OrderHeaderId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "OrderHeaderId",
                table: "Carts");
        }
    }
}
