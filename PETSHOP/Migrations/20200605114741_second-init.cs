using Microsoft.EntityFrameworkCore.Migrations;

namespace PETSHOP.Migrations
{
    public partial class secondinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JWToken",
                table: "Account",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JWToken",
                table: "Account");
        }
    }
}
