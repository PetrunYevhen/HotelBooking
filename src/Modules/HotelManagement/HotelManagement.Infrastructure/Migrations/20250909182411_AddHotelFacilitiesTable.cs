using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagement.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelFacilitiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelFacilities",
                table: "HotelFacilities");

            migrationBuilder.RenameTable(
                name: "HotelFacilities",
                newName: "HotelFacilities",
                newSchema: "Shared");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelFacilities",
                schema: "Shared",
                table: "HotelFacilities",
                columns: new[] { "Id", "FacilityId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelFacilities",
                schema: "Shared",
                table: "HotelFacilities");

            migrationBuilder.RenameTable(
                name: "HotelFacilities",
                schema: "Shared",
                newName: "HotelFacilities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelFacilities",
                table: "HotelFacilities",
                column: "Id");
        }
    }
}
