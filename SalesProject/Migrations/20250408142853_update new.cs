using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesProject.Migrations
{
    /// <inheritdoc />
    public partial class updatenew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompareProducts",
                table: "CompareProducts");

            migrationBuilder.DropIndex(
                name: "IX_CompareProducts_UserId_ProductId",
                table: "CompareProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompareProducts",
                table: "CompareProducts",
                columns: new[] { "UserId", "ProductId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompareProducts",
                table: "CompareProducts");

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
    }
}
