using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Crypto.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addcryptoitemstate",
                columns: table => new
                {
                    correlationid = table.Column<Guid>(type: "uuid", nullable: false),
                    currentstate = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    addcryptoitemtimeouttokenid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_addcryptoitemstate", x => x.correlationid);
                });

            migrationBuilder.CreateTable(
                name: "crypto",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    symbol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    logo = table.Column<string>(type: "text", nullable: true),
                    website = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    sourcecode = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    modifiedon = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    createdon = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crypto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inboxstate",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    messageid = table.Column<Guid>(type: "uuid", nullable: false),
                    consumerid = table.Column<Guid>(type: "uuid", nullable: false),
                    lockid = table.Column<Guid>(type: "uuid", nullable: false),
                    rowversion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    received = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    receivecount = table.Column<int>(type: "integer", nullable: false),
                    expirationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    consumed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastsequencenumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inboxstate", x => x.id);
                    table.UniqueConstraint("ak_inboxstate_messageid_consumerid", x => new { x.messageid, x.consumerid });
                });

            migrationBuilder.CreateTable(
                name: "outboxmessage",
                columns: table => new
                {
                    sequencenumber = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enqueuetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    senttime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    headers = table.Column<string>(type: "text", nullable: true),
                    properties = table.Column<string>(type: "text", nullable: true),
                    inboxmessageid = table.Column<Guid>(type: "uuid", nullable: true),
                    inboxconsumerid = table.Column<Guid>(type: "uuid", nullable: true),
                    outboxid = table.Column<Guid>(type: "uuid", nullable: true),
                    messageid = table.Column<Guid>(type: "uuid", nullable: false),
                    contenttype = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    messagetype = table.Column<string>(type: "text", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    conversationid = table.Column<Guid>(type: "uuid", nullable: true),
                    correlationid = table.Column<Guid>(type: "uuid", nullable: true),
                    initiatorid = table.Column<Guid>(type: "uuid", nullable: true),
                    requestid = table.Column<Guid>(type: "uuid", nullable: true),
                    sourceaddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    destinationaddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    responseaddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    faultaddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    expirationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outboxmessage", x => x.sequencenumber);
                });

            migrationBuilder.CreateTable(
                name: "outboxstate",
                columns: table => new
                {
                    outboxid = table.Column<Guid>(type: "uuid", nullable: false),
                    lockid = table.Column<Guid>(type: "uuid", nullable: false),
                    rowversion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastsequencenumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outboxstate", x => x.outboxid);
                });

            migrationBuilder.CreateTable(
                name: "crypto_price",
                columns: table => new
                {
                    time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    cryptoid = table.Column<Guid>(type: "uuid", nullable: false),
                    cryptoentityid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_crypto_price_crypto_cryptoentityid",
                        column: x => x.cryptoentityid,
                        principalTable: "crypto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "visit",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cryptoid = table.Column<Guid>(type: "uuid", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    createdon = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_visit", x => x.id);
                    table.ForeignKey(
                        name: "fk_visit_crypto_cryptoid",
                        column: x => x.cryptoid,
                        principalTable: "crypto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_crypto_symbol",
                table: "crypto",
                column: "symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_crypto_price_cryptoentityid",
                table: "crypto_price",
                column: "cryptoentityid");

            migrationBuilder.CreateIndex(
                name: "ix_inboxstate_delivered",
                table: "inboxstate",
                column: "delivered");

            migrationBuilder.CreateIndex(
                name: "ix_outboxmessage_enqueuetime",
                table: "outboxmessage",
                column: "enqueuetime");

            migrationBuilder.CreateIndex(
                name: "ix_outboxmessage_expirationtime",
                table: "outboxmessage",
                column: "expirationtime");

            migrationBuilder.CreateIndex(
                name: "ix_outboxmessage_inboxmessageid_inboxconsumerid_sequencenumber",
                table: "outboxmessage",
                columns: new[] { "inboxmessageid", "inboxconsumerid", "sequencenumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_outboxmessage_outboxid_sequencenumber",
                table: "outboxmessage",
                columns: new[] { "outboxid", "sequencenumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_outboxstate_created",
                table: "outboxstate",
                column: "created");

            migrationBuilder.CreateIndex(
                name: "ix_visit_cryptoid",
                table: "visit",
                column: "cryptoid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "addcryptoitemstate");

            migrationBuilder.DropTable(
                name: "crypto_price");

            migrationBuilder.DropTable(
                name: "inboxstate");

            migrationBuilder.DropTable(
                name: "outboxmessage");

            migrationBuilder.DropTable(
                name: "outboxstate");

            migrationBuilder.DropTable(
                name: "visit");

            migrationBuilder.DropTable(
                name: "crypto");
        }
    }
}
