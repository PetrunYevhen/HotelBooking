using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accommodations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPricingDemandScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pricings",
                schema: "Accommodations",
                table: "Pricings");

            migrationBuilder.RenameTable(
                name: "Pricings",
                schema: "Accommodations",
                newName: "Pricing",
                newSchema: "Accommodations");

            migrationBuilder.RenameIndex(
                name: "IX_Pricings_RoomId_IsActive",
                schema: "Accommodations",
                table: "Pricing",
                newName: "IX_Pricing_RoomId_IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_Pricings_RoomId",
                schema: "Accommodations",
                table: "Pricing",
                newName: "IX_Pricing_RoomId");

            migrationBuilder.AddColumn<int>(
                name: "DemandScore",
                schema: "Accommodations",
                table: "Rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pricing",
                schema: "Accommodations",
                table: "Pricing",
                column: "PricingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pricing",
                schema: "Accommodations",
                table: "Pricing");

            migrationBuilder.DropColumn(
                name: "DemandScore",
                schema: "Accommodations",
                table: "Rooms");

            migrationBuilder.RenameTable(
                name: "Pricing",
                schema: "Accommodations",
                newName: "Pricings",
                newSchema: "Accommodations");

            migrationBuilder.RenameIndex(
                name: "IX_Pricing_RoomId_IsActive",
                schema: "Accommodations",
                table: "Pricings",
                newName: "IX_Pricings_RoomId_IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_Pricing_RoomId",
                schema: "Accommodations",
                table: "Pricings",
                newName: "IX_Pricings_RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pricings",
                schema: "Accommodations",
                table: "Pricings",
                column: "PricingId");
        }
    }
}
