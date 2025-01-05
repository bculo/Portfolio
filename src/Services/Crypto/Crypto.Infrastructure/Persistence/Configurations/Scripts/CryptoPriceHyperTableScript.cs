namespace Crypto.Infrastructure.Persistence.Configurations.Scripts;

public static class CryptoPriceHyperTableScript
{
    public static string UpScript => "SELECT create_hypertable('crypto_price', 'time')";

    public static string DownScript => $@"
        CREATE TABLE pg_crypto_price (LIKE {DbTables.CryptoPriceTable.Name} INCLUDING ALL);
        DROP TABLE {DbTables.CryptoPriceTable.Name};
        ALTER TABLE pg_crypto_price RENAME TO {DbTables.CryptoPriceTable.Name};
    ";
}
