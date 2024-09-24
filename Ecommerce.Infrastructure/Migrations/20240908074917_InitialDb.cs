using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductColors");

            migrationBuilder.DropTable(
                name: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "brand_name",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "tracking_number",
                table: "Shipments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "product_code",
                table: "Products",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tracking_number",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "product_code",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "brand_name",
                table: "Products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductColors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color_name = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColors", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_colors_base_entity_id",
                        column: x => x.id,
                        principalTable: "base_entity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_colors_products_product_id",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSizes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    size_value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSizes", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_sizes_base_entity_id",
                        column: x => x.id,
                        principalTable: "base_entity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_sizes_products_product_id",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_product_colors_product_id",
                table: "ProductColors",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_sizes_product_id",
                table: "ProductSizes",
                column: "product_id");
        }
    }
}
