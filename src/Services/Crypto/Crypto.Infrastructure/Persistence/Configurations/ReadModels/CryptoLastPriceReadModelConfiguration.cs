using Crypto.Core.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crypto.Infrastructure.Persistence.Configurations.ReadModels;

public class CryptoLastPriceReadModelConfiguration : IEntityTypeConfiguration<CryptoLastPriceReadModel>
{
    public void Configure(EntityTypeBuilder<CryptoLastPriceReadModel> builder)
    {
        builder.HasNoKey();
        
        builder.ToSqlQuery($@"
				SELECT 
					C.id as cryptoid, 
					C.symbol,
					C.website,
					C.sourcecode,
					C.name,
					TST.lastprice
				FROM {DbTables.CryptoTable.FullName} AS C
				INNER JOIN (
					SELECT 
						cryptoentityid,
						LAST(price, time) AS lastprice
					FROM {DbTables.CryptoPriceTable.FullName} AS CP
					GROUP BY cryptoentityid
				) TST
				ON C.id = TST.cryptoentityid");
    }
}