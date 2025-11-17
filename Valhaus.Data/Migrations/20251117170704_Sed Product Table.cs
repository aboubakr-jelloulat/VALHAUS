using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Valhaus.Data.Migrations
{
    /// <inheritdoc />
    public partial class SedProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "Description", "ListPrice", "Price", "Price100", "Price50", "SKU", "Title" },
                values: new object[,]
                {
                    { 1, "Valhaus", "Low-profile oak coffee table with rounded corners — minimalist Scandinavian design.", 499.0, 449.0, 349.0, 399.0, "VH-CT-001", "Oslo Coffee Table" },
                    { 2, "Valhaus", "Hand-glazed ceramic vase in matte white — understated elegance.", 59.990000000000002, 49.990000000000002, 29.989999999999998, 39.990000000000002, "VH-VS-001", "Nordic Ceramic Vase - Small" },
                    { 3, "Norda", "Corner modular sofa with low profile and wooden base — configurable layout.", 3299.0, 2999.0, 2399.0, 2699.0, "VH-SF-002", "Nord Modular Sofa - Corner" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
