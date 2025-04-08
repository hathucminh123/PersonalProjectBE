using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesProject.Migrations
{
    /// <inheritdoc />
    public partial class updatecompare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompareProducts",
                table: "CompareProducts");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "BlogPosts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompareProducts",
                table: "CompareProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CompareProducts_UserId_ProductId",
                table: "CompareProducts",
                columns: new[] { "UserId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompareProducts",
                table: "CompareProducts");

            migrationBuilder.DropIndex(
                name: "IX_CompareProducts_UserId_ProductId",
                table: "CompareProducts");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "BlogPosts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompareProducts",
                table: "CompareProducts",
                columns: new[] { "UserId", "ProductId" });
        }
    }
}
