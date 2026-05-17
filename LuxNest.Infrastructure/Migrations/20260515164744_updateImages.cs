using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxNest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/VillaImage/royal_villa_card.png");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/VillaImage/premium_pool_villa_card.png");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/VillaImage/luxury_pool_villa_card.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/VillaImage/158817b0-0967-48a9-8809-07ef0ad1fe5a.jpg");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/VillaImage/e801cc6a-c896-4357-aea8-9eb0a25cb596.jpg");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/VillaImage/48678a87-ce4d-4751-acab-b12ad9c06341.jpg");
        }
    }
}
