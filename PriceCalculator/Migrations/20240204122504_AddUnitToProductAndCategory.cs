using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceCalculator.Migrations
{
    public partial class AddUnitToProductAndCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBestPrice",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "UnitType",
                table: "Products",
                newName: "UnitTypeOld");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UnitTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "UnitTypeID",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitTypeID",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitTypeID",
                table: "Products",
                column: "UnitTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UnitTypeID",
                table: "Categories",
                column: "UnitTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_UnitTypes_UnitTypeID",
                table: "Categories",
                column: "UnitTypeID",
                principalTable: "UnitTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitTypes_UnitTypeID",
                table: "Products",
                column: "UnitTypeID",
                principalTable: "UnitTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_UnitTypes_UnitTypeID",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitTypes_UnitTypeID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UnitTypeID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UnitTypeID",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UnitTypeID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitTypeID",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "UnitTypeOld",
                table: "Products",
                newName: "UnitType");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UnitTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBestPrice",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
