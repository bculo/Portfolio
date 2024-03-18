using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypto.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_CryptoWithLastPrice_Viewcs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
				CREATE OR REPLACE VIEW public.crypto_with_last_price AS
				SELECT 
					C.id as cryptoid, 
					C.symbol,
					C.website,
					C.sourcecode,
					C.name,
					TST.lastprice
				FROM public.crypto AS C
				INNER JOIN (
					SELECT 
						cryptoid,
						LAST(price, time) AS lastprice
					FROM public.crypto_price AS CP
					GROUP BY cryptoid
				) TST
				ON C.id = TST.cryptoid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("DROP VIEW IF EXISTS public.crypto_with_last_price");
        }
    }
}
