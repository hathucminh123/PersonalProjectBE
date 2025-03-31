using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesProject.Migrations
{
    /// <inheritdoc />
    public partial class Updatedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscountPrice",
                table: "Products",
                newName: "DiscountAmount");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBestSeller",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "ActiveIngredients",
                table: "Products",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Benefits",
                table: "Products",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ExpShelfLife",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Ingredients",
                table: "Products",
                type: "character varying(3000)",
                maxLength: 3000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainBenefits",
                table: "Products",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PaoShelfLife",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SkinConcerns",
                table: "Products",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SkinType",
                table: "Products",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveIngredients",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Benefits",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ExpShelfLife",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Ingredients",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MainBenefits",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PaoShelfLife",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SkinConcerns",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SkinType",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                table: "Products",
                newName: "DiscountPrice");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBestSeller",
                table: "Products",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }
    }
}
