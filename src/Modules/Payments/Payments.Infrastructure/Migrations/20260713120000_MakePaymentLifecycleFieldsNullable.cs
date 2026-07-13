using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payments.Infrastructure.Migrations;

[DbContext(typeof(PaymentsDbContext))]
[Migration("20260713120000_MakePaymentLifecycleFieldsNullable")]
public sealed class MakePaymentLifecycleFieldsNullable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "CompletedAt",
            schema: "Payments",
            table: "Payments",
            type: "timestamp with time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            UPDATE "Payments"."Payments"
            SET "CompletedAt" = '0001-01-01T00:00:00Z'
            WHERE "CompletedAt" IS NULL;
            """);

        migrationBuilder.AlterColumn<DateTime>(
            name: "CompletedAt",
            schema: "Payments",
            table: "Payments",
            type: "timestamp with time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone",
            oldNullable: true);
    }
}
