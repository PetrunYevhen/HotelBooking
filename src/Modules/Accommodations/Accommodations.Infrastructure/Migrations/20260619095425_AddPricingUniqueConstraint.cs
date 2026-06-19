using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accommodations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPricingUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 CREATE UNIQUE INDEX "IX_Pricing_RoomId_ValidFrom_Type"
                                 ON "Accommodations"."Pricing" ("RoomId", "ValidFrom", "Type");
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 DROP INDEX IF EXISTS "Accommodations"."IX_Pricing_RoomId_ValidFrom_Type";
                                 """);
        }
    }
}
