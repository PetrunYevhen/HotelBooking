using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accommodations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckOutHoursPolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CheckOut",
                schema: "Accommodations",
                table: "Hotels",
                newName: "OperatingHours_End");

            migrationBuilder.RenameColumn(
                name: "CheckIn",
                schema: "Accommodations",
                table: "Hotels",
                newName: "OperatingHours_Start");

            migrationBuilder.AddColumn<int>(
                name: "CheckOutHoursPolicy",
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
                name: "CheckOutHoursPolicy",
                schema: "Accommodations",
                table: "Hotels");

            migrationBuilder.RenameColumn(
                name: "OperatingHours_Start",
                schema: "Accommodations",
                table: "Hotels",
                newName: "CheckIn");

            migrationBuilder.RenameColumn(
                name: "OperatingHours_End",
                schema: "Accommodations",
                table: "Hotels",
                newName: "CheckOut");
        }
    }
}
