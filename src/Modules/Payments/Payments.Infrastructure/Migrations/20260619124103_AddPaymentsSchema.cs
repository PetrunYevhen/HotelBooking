using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payments.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Payments");

            migrationBuilder.RenameTable(
                name: "Payments",
                schema: "PaymentManagement",
                newName: "Payments",
                newSchema: "Payments");

            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                schema: "PaymentManagement",
                newName: "OutboxMessages",
                newSchema: "Payments");

            migrationBuilder.RenameTable(
                name: "InboxMessages",
                schema: "PaymentManagement",
                newName: "InboxMessages",
                newSchema: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "FailureReason",
                schema: "Payments",
                table: "Payments",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalTransactionId",
                schema: "Payments",
                table: "Payments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                schema: "Payments",
                table: "Payments",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                schema: "Payments",
                table: "Payments",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ExternalTransactionId",
                schema: "Payments",
                table: "Payments",
                column: "ExternalTransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status",
                schema: "Payments",
                table: "Payments",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_BookingId",
                schema: "Payments",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ExternalTransactionId",
                schema: "Payments",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_Status",
                schema: "Payments",
                table: "Payments");

            migrationBuilder.EnsureSchema(
                name: "PaymentManagement");

            migrationBuilder.RenameTable(
                name: "Payments",
                schema: "Payments",
                newName: "Payments",
                newSchema: "PaymentManagement");

            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                schema: "Payments",
                newName: "OutboxMessages",
                newSchema: "PaymentManagement");

            migrationBuilder.RenameTable(
                name: "InboxMessages",
                schema: "Payments",
                newName: "InboxMessages",
                newSchema: "PaymentManagement");

            migrationBuilder.AlterColumn<string>(
                name: "FailureReason",
                schema: "PaymentManagement",
                table: "Payments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ExternalTransactionId",
                schema: "PaymentManagement",
                table: "Payments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                schema: "PaymentManagement",
                table: "Payments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);
        }
    }
}
