using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxNest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfRoomsAndGuestsToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfRooms",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfRooms",
                table: "Bookings");
        }
    }
}
