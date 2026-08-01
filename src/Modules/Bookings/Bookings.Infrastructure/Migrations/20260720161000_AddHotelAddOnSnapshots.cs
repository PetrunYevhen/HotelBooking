using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Infrastructure.Migrations;

[Migration("20260720161000_AddHotelAddOnSnapshots")]
public partial class AddHotelAddOnSnapshots : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "HotelAddOnSnapshots",
            schema: "Bookings",
            columns: table => new
            {
                HotelAddOnId = table.Column<Guid>(type: "uuid", nullable: false),
                HotelId = table.Column<Guid>(type: "uuid", nullable: false),
                Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                Price_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                Price_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                PricingType = table.Column<int>(type: "integer", nullable: false),
                IsActive = table.Column<bool>(type: "boolean", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_HotelAddOnSnapshots", x => x.HotelAddOnId));

        migrationBuilder.CreateIndex(
            name: "IX_HotelAddOnSnapshots_HotelId",
            schema: "Bookings",
            table: "HotelAddOnSnapshots",
            column: "HotelId");

        migrationBuilder.AddColumn<Guid>(
            name: "HotelAddOnId",
            schema: "Bookings",
            table: "BookingAddOns",
            type: "uuid",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "PricingType",
            schema: "Bookings",
            table: "BookingAddOns",
            type: "integer",
            nullable: false,
            defaultValue: 1);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "HotelAddOnId", schema: "Bookings", table: "BookingAddOns");
        migrationBuilder.DropColumn(name: "PricingType", schema: "Bookings", table: "BookingAddOns");
        migrationBuilder.DropTable(name: "HotelAddOnSnapshots", schema: "Bookings");
    }
}
