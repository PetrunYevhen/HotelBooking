using Accommodations.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accommodations.Infrastructure.Migrations;

[DbContext(typeof(AccommodationsDbContext))]
[Migration("20260801120000_AddHotelOwner")]
public partial class AddHotelOwner : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "OwnerUserId",
            schema: "Accommodations",
            table: "Hotels",
            type: "uuid",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Hotels_OwnerUserId",
            schema: "Accommodations",
            table: "Hotels",
            column: "OwnerUserId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(name: "IX_Hotels_OwnerUserId", schema: "Accommodations", table: "Hotels");
        migrationBuilder.DropColumn(name: "OwnerUserId", schema: "Accommodations", table: "Hotels");
    }
}
