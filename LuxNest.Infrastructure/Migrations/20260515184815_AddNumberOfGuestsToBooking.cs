using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxNest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfGuestsToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfGuests",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfGuests",
                table: "Bookings");
        }
    }
}
