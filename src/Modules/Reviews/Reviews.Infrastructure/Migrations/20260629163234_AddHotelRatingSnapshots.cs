using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reviews.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelRatingSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotelRatingSnapshots",
                schema: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HotelId = table.Column<Guid>(type: "uuid", nullable: false),
                    AvgRating = table.Column<double>(type: "double precision", nullable: false),
                    LastCalculatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRatingSnapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelRatingSnapshots_HotelId",
                schema: "Reviews",
                table: "HotelRatingSnapshots",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelRatingSnapshots_HotelId_LastCalculatedAt",
                schema: "Reviews",
                table: "HotelRatingSnapshots",
                columns: new[] { "HotelId", "LastCalculatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelRatingSnapshots",
                schema: "Reviews");
        }
    }
}
