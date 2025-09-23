using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagement.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Shared");

            migrationBuilder.EnsureSchema(
                name: "HotelManagement");

            migrationBuilder.CreateTable(
                name: "HotelFacilities",
                columns: table => new
                {
                    HotelId = table.Column<Guid>(type: "uuid", nullable: false),
                    FacilityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelFacilities", x => x.HotelId);
                });

            migrationBuilder.CreateTable(
                name: "HotelRooms",
                schema: "Shared",
                columns: table => new
                {
                    HotelId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRooms", x => new { x.HotelId, x.RoomId });
                });

            migrationBuilder.CreateTable(
                name: "Hotels",
                schema: "HotelManagement",
                columns: table => new
                {
                    HotelId = table.Column<Guid>(type: "uuid", nullable: false),
                    HotelName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: false),
                    MinRoomPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.HotelId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelFacilities");

            migrationBuilder.DropTable(
                name: "HotelRooms",
                schema: "Shared");

            migrationBuilder.DropTable(
                name: "Hotels",
                schema: "HotelManagement");
        }
    }
}
