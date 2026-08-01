using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Infrastructure.Migrations;

[DbContext(typeof(BookingDbContext))]
[Migration("20260713120300_AddBookingCompletionReason")]
public sealed class AddBookingCompletionReason : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "CompletionReason",
            schema: "Bookings",
            table: "Bookings",
            type: "integer",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CompletionReason",
            schema: "Bookings",
            table: "Bookings");
    }
}
