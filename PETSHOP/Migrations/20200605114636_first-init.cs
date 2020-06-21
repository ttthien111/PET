using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PETSHOP.Migrations
{
    public partial class firstinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillDetail");

            migrationBuilder.DropTable(
                name: "CostumeProduct");

            migrationBuilder.DropTable(
                name: "DeliveryProduct");

            migrationBuilder.DropTable(
                name: "FoodProduct");

            migrationBuilder.DropTable(
                name: "InventoryReceivingNoteDetail_ForToy");

            migrationBuilder.DropTable(
                name: "ToyProduct");

            migrationBuilder.DropTable(
                name: "UserComment");

            migrationBuilder.DropTable(
                name: "UserScore");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "DeliveryProduct_State");

            migrationBuilder.DropTable(
                name: "DeliveryProductType");

            migrationBuilder.DropTable(
                name: "InventoryReceivingNote");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "PaymentMethodType");

            migrationBuilder.DropTable(
                name: "InventoryReceivingNoteDetail_ForCostume");

            migrationBuilder.DropTable(
                name: "InventoryReceivingNoteDetail_ForFood");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Distributor");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "CustomerType");

            migrationBuilder.DropTable(
                name: "AccountRole");
        }
    }
}
