using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Infrastructure.Migrations;

[Migration("20260720113000_AddBookingAddOns")]
public partial class AddBookingAddOns : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "BookingAddOns",
            schema: "Bookings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                Quantity = table.Column<int>(type: "integer", nullable: false),
                UnitPrice_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                UnitPrice_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                TotalPrice_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                TotalPrice_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BookingAddOns", x => x.Id);
                table.ForeignKey(
                    name: "FK_BookingAddOns_Bookings_BookingId",
                    column: x => x.BookingId,
                    principalSchema: "Bookings",
                    principalTable: "Bookings",
                    principalColumn: "BookingId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_BookingAddOns_BookingId",
            schema: "Bookings",
            table: "BookingAddOns",
            column: "BookingId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "BookingAddOns", schema: "Bookings");
    }
}
