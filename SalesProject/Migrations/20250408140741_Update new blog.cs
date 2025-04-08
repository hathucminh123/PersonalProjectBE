using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SalesProject.Migrations
{
    /// <inheritdoc />
    public partial class Updatenewblog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompareProducts",
                table: "CompareProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompareProducts",
                table: "CompareProducts",
                columns: new[] { "UserId", "ProductId" });

            migrationBuilder.CreateTable(
                name: "BlogCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogSubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    BlogCategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogSubCategories_BlogCategories_BlogCategoryId",
                        column: x => x.BlogCategoryId,
                        principalTable: "BlogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    BlogSubCategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPosts_BlogSubCategories_BlogSubCategoryId",
                        column: x => x.BlogSubCategoryId,
                        principalTable: "BlogSubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompareProducts_ProductId",
                table: "CompareProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_BlogSubCategoryId",
                table: "BlogPosts",
                column: "BlogSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogSubCategories_BlogCategoryId",
                table: "BlogSubCategories",
                column: "BlogCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompareProducts_Products_ProductId",
                table: "CompareProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompareProducts_Users_UserId",
                table: "CompareProducts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompareProducts_Products_ProductId",
                table: "CompareProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CompareProducts_Users_UserId",
                table: "CompareProducts");

            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "BlogSubCategories");

            migrationBuilder.DropTable(
                name: "BlogCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompareProducts",
                table: "CompareProducts");

            migrationBuilder.DropIndex(
                name: "IX_CompareProducts_ProductId",
                table: "CompareProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompareProducts",
                table: "CompareProducts",
                column: "Id");
        }
    }
}
