using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations;

[DbContext(typeof(UsersDbContext))]
[Migration("20260731170000_InitialUsers")]
public partial class InitialUsers : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "Accounts");
        migrationBuilder.EnsureSchema(name: "Identity");

        migrationBuilder.CreateTable(
            name: "InboxMessages",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                OccurredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Type = table.Column<string>(type: "text", nullable: false),
                Data = table.Column<string>(type: "text", nullable: false),
                ProcessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_InboxMessages", x => x.Id));

        migrationBuilder.CreateTable(
            name: "OutboxMessages",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                OccurredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Type = table.Column<string>(type: "text", nullable: false),
                Data = table.Column<string>(type: "text", nullable: false),
                ProcessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_OutboxMessages", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Users",
            schema: "Accounts",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                PasswordHash = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                Role = table.Column<int>(type: "integer", nullable: false),
                RefreshTokenHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                RefreshTokenExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                PhoneNumber = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Users", x => x.UserId));

        migrationBuilder.CreateIndex(name: "IX_Users_Email", schema: "Accounts", table: "Users", column: "Email", unique: true);
        migrationBuilder.CreateIndex(name: "IX_Users_Username", schema: "Accounts", table: "Users", column: "Username", unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "InboxMessages", schema: "Identity");
        migrationBuilder.DropTable(name: "OutboxMessages", schema: "Identity");
        migrationBuilder.DropTable(name: "Users", schema: "Accounts");
    }
}
