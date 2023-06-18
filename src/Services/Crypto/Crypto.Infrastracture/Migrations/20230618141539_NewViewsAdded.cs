using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypto.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class NewViewsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InboxState_MessageId_ConsumerId",
                table: "InboxState");

            migrationBuilder.AddColumn<Guid>(
                name: "LockId",
                table: "OutboxState",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "OutboxState",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LockId",
                table: "InboxState",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "InboxState",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_InboxState_MessageId_ConsumerId",
                table: "InboxState",
                columns: new[] { "MessageId", "ConsumerId" });

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW dbo.CryptoLastPrice
                WITH SCHEMABINDING
                AS

                WITH LastCryptoPrice AS (
	                SELECT 
		                P.CryptoId AS CryptoId, 
		                P.Price AS Price,
		                ROW_NUMBER() OVER(
			                PARTITION BY P.CryptoID
		                    ORDER BY P.CreatedOn DESC
		                ) AS RowNumber
	                FROM dbo.Prices AS P
                )

                SELECT 
                    C.Id,
	                C.Symbol,
	                C.Name, 
	                C.Description, 
	                C.WebSite, 
	                C.SourceCode, 
	                C.CreatedOn AS Created, 
	                C.Logo,
	                LCP.Price
                FROM dbo.Cryptos AS C
                LEFT JOIN LastCryptoPrice AS LCP
	                ON LCP.CryptoId = C.Id
                WHERE RowNumber = 1
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_InboxState_MessageId_ConsumerId",
                table: "InboxState");

            migrationBuilder.DropColumn(
                name: "LockId",
                table: "OutboxState");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "OutboxState");

            migrationBuilder.DropColumn(
                name: "LockId",
                table: "InboxState");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "InboxState");

            migrationBuilder.CreateIndex(
                name: "IX_InboxState_MessageId_ConsumerId",
                table: "InboxState",
                columns: new[] { "MessageId", "ConsumerId" },
                unique: true);

            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.CryptoLastPrice");
        }
    }
}
