using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accommodations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Accommodations");

            migrationBuilder.CreateTable(
                name: "Hotels",
                schema: "Accommodations",
                columns: table => new
                {
                    HotelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: false),
                    MinRoomPrice_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    MinRoomPrice_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Street = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CheckIn = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    CheckOut = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.HotelId);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "Accommodations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pricings",
                schema: "Accommodations",
                columns: table => new
                {
                    PricingId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Price_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Price_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    ValidTo = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pricings", x => x.PricingId);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                schema: "Accommodations",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    HotelId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Beds = table.Column<int>(type: "integer", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    BasePrice_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BasePrice_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "HotelFacilities",
                schema: "Accommodations",
                columns: table => new
                {
                    HotelFacilityId = table.Column<Guid>(type: "uuid", nullable: false),
                    HotelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelFacilities", x => x.HotelFacilityId);
                    table.ForeignKey(
                        name: "FK_HotelFacilities_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalSchema: "Accommodations",
                        principalTable: "Hotels",
                        principalColumn: "HotelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomFacilities",
                schema: "Accommodations",
                columns: table => new
                {
                    RoomFacilityId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomFacilities", x => x.RoomFacilityId);
                    table.ForeignKey(
                        name: "FK_RoomFacilities_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Accommodations",
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelFacilities_Category",
                schema: "Accommodations",
                table: "HotelFacilities",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_HotelFacilities_HotelId",
                schema: "Accommodations",
                table: "HotelFacilities",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_Rating",
                schema: "Accommodations",
                table: "Hotels",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_Status",
                schema: "Accommodations",
                table: "Hotels",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Pricings_RoomId",
                schema: "Accommodations",
                table: "Pricings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Pricings_RoomId_IsActive",
                schema: "Accommodations",
                table: "Pricings",
                columns: new[] { "RoomId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_RoomFacilities_Category",
                schema: "Accommodations",
                table: "RoomFacilities",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_RoomFacilities_RoomId",
                schema: "Accommodations",
                table: "RoomFacilities",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelId",
                schema: "Accommodations",
                table: "Rooms",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelId_RoomNumber",
                schema: "Accommodations",
                table: "Rooms",
                columns: new[] { "HotelId", "RoomNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_IsActive",
                schema: "Accommodations",
                table: "Rooms",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_Status",
                schema: "Accommodations",
                table: "Rooms",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelFacilities",
                schema: "Accommodations");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "Accommodations");

            migrationBuilder.DropTable(
                name: "Pricings",
                schema: "Accommodations");

            migrationBuilder.DropTable(
                name: "RoomFacilities",
                schema: "Accommodations");

            migrationBuilder.DropTable(
                name: "Hotels",
                schema: "Accommodations");

            migrationBuilder.DropTable(
                name: "Rooms",
                schema: "Accommodations");
        }
    }
}
