using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelAddOnSnapshotCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS "Bookings"."BookingAddOns" (
                    "Id" uuid NOT NULL,
                    "BookingId" uuid NOT NULL,
                    "Code" character varying(50) NOT NULL,
                    "Name" character varying(120) NOT NULL,
                    "Quantity" integer NOT NULL,
                    "UnitPrice_Amount" numeric(18,2) NOT NULL,
                    "UnitPrice_Currency" character varying(3) NOT NULL,
                    "TotalPrice_Amount" numeric(18,2) NOT NULL,
                    "TotalPrice_Currency" character varying(3) NOT NULL,
                    CONSTRAINT "PK_BookingAddOns" PRIMARY KEY ("Id")
                );
                """);

            migrationBuilder.Sql("""
                ALTER TABLE "Bookings"."BookingAddOns"
                ADD COLUMN IF NOT EXISTS "HotelAddOnId" uuid;
                ALTER TABLE "Bookings"."BookingAddOns"
                ADD COLUMN IF NOT EXISTS "PricingType" integer NOT NULL DEFAULT 0;
                """);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // BookingAddOns and HotelAddOnSnapshots were introduced by the preceding migrations.
        }
    }
}
