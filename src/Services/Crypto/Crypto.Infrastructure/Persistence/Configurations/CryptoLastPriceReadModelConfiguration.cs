using Crypto.Core.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crypto.Infrastructure.Persistence.Configurations;

public class CryptoLastPriceReadModelConfiguration : IEntityTypeConfiguration<CryptoLastPriceReadModel>
{
    public void Configure(EntityTypeBuilder<CryptoLastPriceReadModel> builder)
    {
        builder.HasNoKey();
        builder.ToView("crypto_with_last_price");
    }
}