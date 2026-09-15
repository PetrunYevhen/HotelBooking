using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace Accommodations.Infrastructure.Migrations;

[DbContext(typeof(AccommodationsDbContext))]
[Migration("20260906121000_OwnHotelierApplications")]
public partial class OwnHotelierApplications : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema("Accommodations");
        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF to_regclass('"Accounts"."HotelierApplications"') IS NOT NULL THEN
                    ALTER TABLE "Accounts"."HotelierApplications" SET SCHEMA "Accommodations";
                END IF;
            END $$;
            CREATE TABLE IF NOT EXISTS "Accommodations"."HotelierApplications" (
                "HotelierApplicationId" uuid PRIMARY KEY,
                "ApplicantId" uuid NOT NULL,
                "LegalBusinessName" varchar(200) NOT NULL,
                "RegistrationNumber" varchar(100) NOT NULL,
                "TaxNumber" varchar(100),
                "BusinessEmail" varchar(320) NOT NULL,
                "BusinessPhoneNumber" varchar(32) NOT NULL,
                "FirstPropertyName" varchar(200) NOT NULL,
                "FirstPropertyAddress" varchar(500) NOT NULL,
                "Status" integer NOT NULL,
                "SubmittedAt" timestamptz NOT NULL,
                "ReviewedAt" timestamptz,
                "ReviewedByUserId" uuid,
                "RejectionReason" varchar(1000)
            );
            CREATE UNIQUE INDEX IF NOT EXISTS "IX_HotelierApplications_ApplicantId_Status"
                ON "Accommodations"."HotelierApplications" ("ApplicantId", "Status") WHERE "Status" = 0;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema("Accounts");
        migrationBuilder.Sql("""ALTER TABLE "Accommodations"."HotelierApplications" SET SCHEMA "Accounts";""");
    }
}
