using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accommodations.Infrastructure.Migrations;

[Migration("20260720160000_AddHotelAddOns")]
public partial class AddHotelAddOns : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "HotelAddOns",
            schema: "Accommodations",
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
                IsActive = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_HotelAddOns", x => x.HotelAddOnId));

        migrationBuilder.CreateIndex(
            name: "IX_HotelAddOns_HotelId_Code",
            schema: "Accommodations",
            table: "HotelAddOns",
            columns: new[] { "HotelId", "Code" },
            unique: true);

        migrationBuilder.Sql("""
            INSERT INTO "Accommodations"."HotelAddOns"
                ("HotelAddOnId", "HotelId", "Code", "Name", "Description", "Price_Amount", "Price_Currency", "PricingType", "IsActive")
            SELECT md5(h."HotelId"::text || ':' || d."Code")::uuid,
                   h."HotelId", d."Code", d."Name", d."Description", d."Price", 'EUR', d."PricingType", true
            FROM "Accommodations"."Hotels" h
            CROSS JOIN (VALUES
                ('airport-transfer', 'Airport transfer', 'Private transfer to or from the airport', 45.00, 1),
                ('romantic-package', 'Romantic package', 'A bottle of sparkling wine and a room surprise', 75.00, 1),
                ('breakfast-buffet', 'Breakfast buffet', 'Fresh breakfast served daily', 18.00, 3)
            ) AS d("Code", "Name", "Description", "Price", "PricingType")
            ON CONFLICT ("HotelId", "Code") DO NOTHING;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "HotelAddOns", schema: "Accommodations");
    }
}
