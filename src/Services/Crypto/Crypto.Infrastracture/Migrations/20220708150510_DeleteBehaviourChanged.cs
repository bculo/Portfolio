using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypto.Infrastracture.Migrations
{
    public partial class DeleteBehaviourChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CryptoExplorer_Cryptos_CryptoId",
                table: "CryptoExplorer");

            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Cryptos_CryptoId",
                table: "Prices");

            migrationBuilder.AddForeignKey(
                name: "FK_CryptoExplorer_Cryptos_CryptoId",
                table: "CryptoExplorer",
                column: "CryptoId",
                principalTable: "Cryptos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Cryptos_CryptoId",
                table: "Prices",
                column: "CryptoId",
                principalTable: "Cryptos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CryptoExplorer_Cryptos_CryptoId",
                table: "CryptoExplorer");

            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Cryptos_CryptoId",
                table: "Prices");

            migrationBuilder.AddForeignKey(
                name: "FK_CryptoExplorer_Cryptos_CryptoId",
                table: "CryptoExplorer",
                column: "CryptoId",
                principalTable: "Cryptos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Cryptos_CryptoId",
                table: "Prices",
                column: "CryptoId",
                principalTable: "Cryptos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
