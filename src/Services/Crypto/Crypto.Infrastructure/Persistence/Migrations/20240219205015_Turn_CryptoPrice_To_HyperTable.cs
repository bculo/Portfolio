using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypto.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Turn_CryptoPrice_To_HyperTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SELECT create_hypertable('crypto_price', 'time');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE TABLE pg_crypto_price (LIKE crypto_price INCLUDING ALL);");
            migrationBuilder.Sql("DROP TABLE crypto_price;");
            migrationBuilder.Sql("ALTER TABLE pg_crypto_price RENAME TO crypto_price;");
        }
    }
}
