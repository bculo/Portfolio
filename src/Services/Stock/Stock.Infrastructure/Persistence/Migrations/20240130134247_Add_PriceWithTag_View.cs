using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_PriceWithTag_View : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW public.stock_with_price_tag AS
                SELECT s.id as stockid,
                 s.symbol as symbol,
                 CASE 
                  WHEN spo.price is NULL THEN -1
                  ELSE spo.price
                 END as price
                FROM public.stocks AS s
                LEFT JOIN LATERAL
                 (SELECT
                  sp.price
                  FROM public.stocks_prices AS sp
                  WHERE sp.stockid = s.id
                  ORDER BY sp.createdat
                  LIMIT 1) AS spo ON true
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS public.stock_with_price_tag");
        }
    }
}
