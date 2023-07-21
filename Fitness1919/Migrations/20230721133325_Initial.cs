using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness1919.Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PruchaseDate",
                table: "ShoppingCarts",
                newName: "PurchaseDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseDate",
                table: "ShoppingCarts",
                newName: "PruchaseDate");
        }
    }
}
