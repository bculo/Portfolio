using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                name: "crypto",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    symbol = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    logo = table.Column<string>(type: "text", nullable: false),
                    website = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    sourcecode = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    modifiedon = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdon = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crypto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "crypto_price",
                columns: table => new
                {
                    time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    cryptoid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fk_crypto_price_crypto_cryptoid",
                        column: x => x.cryptoid,
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
                    modifiedon = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdon = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_visit", x => x.id);
                    table.ForeignKey(
                        name: "fk_visit_cryptos_cryptoid",
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
                name: "ix_crypto_price_cryptoid",
                table: "crypto_price",
                column: "cryptoid");

            migrationBuilder.CreateIndex(
                name: "ix_visit_cryptoid",
                table: "visit",
                column: "cryptoid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "crypto_price");

            migrationBuilder.DropTable(
                name: "visit");

            migrationBuilder.DropTable(
                name: "crypto");
        }
    }
}
