namespace Crypto.Infrastructure.Persistence.Configurations.Scripts;

public static class CryptoWithPriceFunctionScript
{
    public static string UpScript => $@"   
        CREATE OR REPLACE FUNCTION {DbFunctions.CryptoPriceTimeFrame.Name}(notoldermin integer, timebucketmin integer)
            RETURNS TABLE
            (
                cryptoid uuid,
                symbol character varying(15),
                website character varying(250),
                sourcecode character varying(250),
                name character varying(250),
                timebucket timestamp with time zone,
                minprice numeric(18,2),
                maxprice numeric(18,2),
                avgprice numeric(18,2),
                lastprice numeric(18,2)
            )
            LANGUAGE SQL
            AS 
            $$
                SELECT 
                C.id as cryptoid, 
                C.symbol,
                C.website,
                C.sourcecode,
                C.name,
                TST.timebucket,
                TST.minprice,
                TST.maxprice,
                TST.avgprice,
                TST.lastprice
                    FROM {DbTables.CryptoTable.FullName} AS C
                INNER JOIN (
                    SELECT 
                        time_bucket('1 min'::INTERVAL * timebucketmin, time) AS timebucket, 
                        cryptoid,
                        COUNT(*) AS counter,
                        AVG(price) AS avgprice,
                        MIN(price) AS minprice,
                        MAX(price) AS maxprice,
                        LAST(price, time) AS lastprice
                    FROM {DbTables.CryptoPriceTable.FullName} AS CP
                    WHERE time > now() - '1 min'::INTERVAL * notoldermin
                    GROUP BY timebucket, cryptoid
                ) TST
                ON C.id = TST.cryptoid
                ORDER BY C.symbol, TST.timebucket;
            $$;
    ";
    
    public static string DownScript => "DROP FUNCTION IF EXISTS get_data_by_timeframe(integer, integer)";
}