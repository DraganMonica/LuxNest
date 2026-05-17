using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxNest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVillaDescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Nestled among lush tropical gardens, the Royal Villa offers a serene escape with a private pool, sun-drenched balcony, and a warm king-size bedroom. Ideal for couples or small families seeking nature, privacy, and charm.");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "An architectural showpiece that comes alive at night, the Premium Pool Villa features a stunning illuminated pool, spacious interiors, and flexible sleeping arrangements for up to 4 guests. Perfect for those who love to entertain under the stars.");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "The pinnacle of modern luxury. Floor-to-ceiling glass walls frame breathtaking panoramic views, while a private pool and jacuzzi await just outside. With 750 sqft of refined living space, the Luxury Pool Villa is an unparalleled retreat for the discerning traveller.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.");
        }
    }
}
