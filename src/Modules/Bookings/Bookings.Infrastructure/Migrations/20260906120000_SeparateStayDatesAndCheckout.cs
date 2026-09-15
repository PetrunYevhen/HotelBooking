using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookings.Infrastructure.Migrations;

[DbContext(typeof(BookingDbContext))]
[Migration("20260906120000_SeparateStayDatesAndCheckout")]
public sealed class SeparateStayDatesAndCheckout : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>("ScheduledCheckOutAt", "Bookings", schema: "Bookings",
            type: "timestamp with time zone", nullable: true);
        migrationBuilder.AddColumn<bool>("LatePaymentRefundRequested", "Bookings", schema: "Bookings",
            type: "boolean", nullable: false, defaultValue: false);
        migrationBuilder.Sql("""
            UPDATE "Bookings"."Bookings"
            SET "ScheduledCheckOutAt" = "CheckOut",
                "CheckIn" = date_trunc('day', "CheckIn" AT TIME ZONE 'UTC') AT TIME ZONE 'UTC',
                "CheckOut" = date_trunc('day', "CheckOut" AT TIME ZONE 'UTC') AT TIME ZONE 'UTC';
            ALTER TABLE "Bookings"."Bookings" ALTER COLUMN "ScheduledCheckOutAt" SET NOT NULL;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""UPDATE "Bookings"."Bookings" SET "CheckOut" = "ScheduledCheckOutAt";""");
        migrationBuilder.DropColumn("ScheduledCheckOutAt", "Bookings", "Bookings");
        migrationBuilder.DropColumn("LatePaymentRefundRequested", "Bookings", "Bookings");
    }
}
