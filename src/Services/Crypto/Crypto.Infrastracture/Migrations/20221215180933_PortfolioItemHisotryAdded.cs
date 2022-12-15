using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypto.Infrastracture.Migrations
{
    public partial class PortfolioItemHisotryAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "PortfolioItems");

            migrationBuilder.AddColumn<long>(
                name: "CryptoId",
                table: "PortfolioItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Cryptos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cryptos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "PortfolioItemHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(2,2)", precision: 2, nullable: false),
                    PortfolioItemId = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioItemHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortfolioItemHistory_PortfolioItems_PortfolioItemId",
                        column: x => x.PortfolioItemId,
                        principalTable: "PortfolioItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioItems_CryptoId",
                table: "PortfolioItems",
                column: "CryptoId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioItemHistory_PortfolioItemId",
                table: "PortfolioItemHistory",
                column: "PortfolioItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioItems_Cryptos_CryptoId",
                table: "PortfolioItems",
                column: "CryptoId",
                principalTable: "Cryptos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioItems_Cryptos_CryptoId",
                table: "PortfolioItems");

            migrationBuilder.DropTable(
                name: "PortfolioItemHistory");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioItems_CryptoId",
                table: "PortfolioItems");

            migrationBuilder.DropColumn(
                name: "CryptoId",
                table: "PortfolioItems");

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "PortfolioItems",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Cryptos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cryptos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
