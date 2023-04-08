using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypto.Infrastracture.Migrations
{
    public partial class masstransitstatemachinefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OutboxMessage_SequenceNumber_OutboxId",
                table: "OutboxMessage");

            migrationBuilder.CreateTable(
                name: "AddCryptoItemState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AddCryptoItemTimeoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddCryptoItemState", x => x.CorrelationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_OutboxId_SequenceNumber",
                table: "OutboxMessage",
                columns: new[] { "OutboxId", "SequenceNumber" },
                unique: true,
                filter: "[OutboxId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddCryptoItemState");

            migrationBuilder.DropIndex(
                name: "IX_OutboxMessage_OutboxId_SequenceNumber",
                table: "OutboxMessage");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_SequenceNumber_OutboxId",
                table: "OutboxMessage",
                columns: new[] { "SequenceNumber", "OutboxId" },
                unique: true,
                filter: "[OutboxId] IS NOT NULL");
        }
    }
}
