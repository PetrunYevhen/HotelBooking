using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accommodations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelPolicies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PetPolicy",
                schema: "Accommodations",
                table: "Hotels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Policies_CancellationType",
                schema: "Accommodations",
                table: "Hotels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Policies_Cancellation_DeadlineDays",
                schema: "Accommodations",
                table: "Hotels",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Policies_Cancellation_PenaltyPercentage",
                schema: "Accommodations",
                table: "Hotels",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SmokingPolicy",
                schema: "Accommodations",
                table: "Hotels",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PetPolicy",
                schema: "Accommodations",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Policies_CancellationType",
                schema: "Accommodations",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Policies_Cancellation_DeadlineDays",
                schema: "Accommodations",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Policies_Cancellation_PenaltyPercentage",
                schema: "Accommodations",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "SmokingPolicy",
                schema: "Accommodations",
                table: "Hotels");
        }
    }
}
