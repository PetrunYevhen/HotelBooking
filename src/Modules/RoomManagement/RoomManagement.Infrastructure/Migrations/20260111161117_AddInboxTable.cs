using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInboxTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "InboxMessages",
                newName: "InboxMessages",
                newSchema: "RoomManagement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "InboxMessages",
                schema: "RoomManagement",
                newName: "InboxMessages");
        }
    }
}
