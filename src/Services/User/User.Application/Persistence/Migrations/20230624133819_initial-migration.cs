using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace User.Application.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "administration");

            migrationBuilder.CreateTable(
                name: "portfoliousers",
                schema: "administration",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    firstname = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    lastname = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    bornon = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_portfoliousers", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_portfoliousers_username",
                schema: "administration",
                table: "portfoliousers",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "portfoliousers",
                schema: "administration");
        }
    }
}
