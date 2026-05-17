using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxNest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateVillaImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://placehold.co/600x400");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://placehold.co/600x401");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://placehold.co/600x402");
        }
    }
}
