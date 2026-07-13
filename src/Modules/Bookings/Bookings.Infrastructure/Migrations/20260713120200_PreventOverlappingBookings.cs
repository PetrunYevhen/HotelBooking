using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Infrastructure.Migrations;

[DbContext(typeof(BookingDbContext))]
[Migration("20260713120200_PreventOverlappingBookings")]
public sealed class PreventOverlappingBookings : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS btree_gist;");
        migrationBuilder.Sql(
            """
            ALTER TABLE "Bookings"."Bookings"
            ADD CONSTRAINT "EX_Bookings_RoomId_Dates"
            EXCLUDE USING gist
            (
                "RoomId" WITH =,
                tstzrange("CheckIn", "CheckOut", '[)') WITH &&
            )
            WHERE ("Status" <> 3);
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            ALTER TABLE "Bookings"."Bookings"
            DROP CONSTRAINT IF EXISTS "EX_Bookings_RoomId_Dates";
            """);
    }
}
