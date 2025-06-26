using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductService.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert categories
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 1, "Clothing", null },
                    { 2, "Footwear", null },
                    { 3, "Accessories", null },
                    { 4, "T-Shirts", 1 },      // Child category of Clothing
                    { 5, "Sneakers", 2 }       // Child category of Footwear
                });

            // Insert products
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Price", "Description", "MainImageUrl", "CategoryId" },
                values: new object[,]
                {
                    { 1, "Basic Tee", 19.99m, "Plain cotton T-Shirt", "https://example.com/images/basic-tee.jpg", 4 },
                    { 2, "Sport Sneakers", 59.99m, "Comfortable running sneakers", "https://example.com/images/sport-sneakers.jpg", 5 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete inserted data
            migrationBuilder.DeleteData(table: "Products", keyColumn: "Id", keyValues: new object[] { 1, 2 });
            migrationBuilder.DeleteData(table: "Categories", keyColumn: "Id", keyValues: new object[] { 1, 2, 3, 4, 5 });
        }
    }
}
