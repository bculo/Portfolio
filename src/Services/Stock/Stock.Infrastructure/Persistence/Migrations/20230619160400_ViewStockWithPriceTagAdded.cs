using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ViewStockWithPriceTagAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW assets.stockwithpricetag AS
                SELECT s.id,
	                s.symbol,
	                CASE 
		                WHEN spo.price is NULL THEN -1
		                ELSE spo.price
	                END
                FROM assets.stock AS s
                LEFT JOIN LATERAL
	                (SELECT
		                sp.price
		                FROM assets.stockprice AS sp
		                WHERE sp.stockid = s.id
		                ORDER BY sp.createdat
		                LIMIT 1) AS spo ON true
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS assets.stockwithpricetag");
        }
    }
}
