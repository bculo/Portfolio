using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypto.Infrastracture.Migrations
{
    public partial class DatabaseTableNamingFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioItem_Portfolio_PortfolioId",
                table: "PortfolioItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioItem",
                table: "PortfolioItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Portfolio",
                table: "Portfolio");

            migrationBuilder.RenameTable(
                name: "PortfolioItem",
                newName: "PortfolioItems");

            migrationBuilder.RenameTable(
                name: "Portfolio",
                newName: "Portfolios");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioItem_PortfolioId",
                table: "PortfolioItems",
                newName: "IX_PortfolioItems_PortfolioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioItems",
                table: "PortfolioItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Portfolios",
                table: "Portfolios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioItems_Portfolios_PortfolioId",
                table: "PortfolioItems",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioItems_Portfolios_PortfolioId",
                table: "PortfolioItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Portfolios",
                table: "Portfolios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioItems",
                table: "PortfolioItems");

            migrationBuilder.RenameTable(
                name: "Portfolios",
                newName: "Portfolio");

            migrationBuilder.RenameTable(
                name: "PortfolioItems",
                newName: "PortfolioItem");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioItems_PortfolioId",
                table: "PortfolioItem",
                newName: "IX_PortfolioItem_PortfolioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Portfolio",
                table: "Portfolio",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioItem",
                table: "PortfolioItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioItem_Portfolio_PortfolioId",
                table: "PortfolioItem",
                column: "PortfolioId",
                principalTable: "Portfolio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
