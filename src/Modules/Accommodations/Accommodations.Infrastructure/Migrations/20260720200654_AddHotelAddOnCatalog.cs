using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accommodations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelAddOnCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLiked",
                schema: "Accommodations",
                table: "Hotels",
                type: "boolean",
                nullable: false,
                defaultValue: false);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLiked",
                schema: "Accommodations",
                table: "Hotels");
        }
    }
}
