using Microsoft.EntityFrameworkCore.Migrations;

namespace Mango.Services.ProductAPI.Migrations
{
    public partial class SeedDataProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryName", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 1, "Shoes", "Hello world and hehe", "https://cf.shopee.vn/file/a7c893d688c8430db3f118c00319fc9a", "Shoes 1", 2000.0 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryName", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 2, "Shoes", "Hello world and hehe 222 ", "https://giayxshop.vn/wp-content/uploads/2021/01/z2261641090407_9a527dfa37fa44a1b8cfd0fd14d1ca77-scaled.jpg", "Shoes 2", 2000.0 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryName", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 3, "Ball", "Manchester United", "https://www.hidosport.com/uploads/5/4/1/0/54105107/manchester-united-18-19-home-kit-2-3_orig.jpg", "Ball hehe", 2000.0 });
        }

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
