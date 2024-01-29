using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Stock.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "stocks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    modifiedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stocks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stocks_prices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    stockid = table.Column<int>(type: "integer", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stocks_prices", x => x.id);
                    table.ForeignKey(
                        name: "fk_stocks_prices_stocks_stockid",
                        column: x => x.stockid,
                        principalTable: "stocks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_stocks_symbol",
                table: "stocks",
                column: "symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_stocks_prices_stockid",
                table: "stocks_prices",
                column: "stockid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stocks_prices");

            migrationBuilder.DropTable(
                name: "stocks");
        }
    }
}
