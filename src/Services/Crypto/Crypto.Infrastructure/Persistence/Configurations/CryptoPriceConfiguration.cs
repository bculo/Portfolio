using Crypto.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crypto.Infrastructure.Persistence.Configurations
{
    public class CryptoPriceConfiguration : IEntityTypeConfiguration<CryptoPrice>
    {
        public const string TableName = "crypto_price";
        
        public void Configure(EntityTypeBuilder<CryptoPrice> builder)
        {
            builder.HasNoKey();

            builder.HasOne(i => i.Crypto);

            builder.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();
            
            
            builder.ToTable(TableName);
        }
    }
}
