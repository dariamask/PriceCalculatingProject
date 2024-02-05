using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceCalculator.Migrations
{
    public partial class DeleteOldUnitTypeProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitTypeOld",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryUnitType",
                table: "Categories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitTypeOld",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryUnitType",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
