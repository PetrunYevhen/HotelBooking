using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notifications.Infrastructure.Migrations;

[DbContext(typeof(NotificationsDbContext))]
[Migration("20260713120100_MakeNotificationSentAtNullable")]
public sealed class MakeNotificationSentAtNullable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "SentAt",
            schema: "Notifications",
            table: "Notifications",
            type: "timestamp with time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            UPDATE "Notifications"."Notifications"
            SET "SentAt" = "CreatedAt"
            WHERE "SentAt" IS NULL;
            """);

        migrationBuilder.AlterColumn<DateTime>(
            name: "SentAt",
            schema: "Notifications",
            table: "Notifications",
            type: "timestamp with time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone",
            oldNullable: true);
    }
}
