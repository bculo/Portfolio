using Crypto.Infrastructure.Persistence.Configurations.Scripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypto.Infrastructure.Persistence.Migrations.Manual
{
    /// <inheritdoc />
    public partial class Migration_001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(CryptoPriceHyperTableScript.UpScript);
            migrationBuilder.Sql(CryptoWithPriceFunctionScript.UpScript);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(CryptoPriceHyperTableScript.DownScript);
            migrationBuilder.Sql(CryptoWithPriceFunctionScript.DownScript);
        }

    }
}
