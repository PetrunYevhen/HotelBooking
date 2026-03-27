using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                newName: "OutboxMessages",
                newSchema: "PaymentManagement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                schema: "PaymentManagement",
                newName: "OutboxMessages");
        }
    }
}
