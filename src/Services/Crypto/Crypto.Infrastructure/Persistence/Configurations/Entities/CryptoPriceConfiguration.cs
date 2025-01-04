using Crypto.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crypto.Infrastructure.Persistence.Configurations.Entities
{
    public class CryptoPriceConfiguration : IEntityTypeConfiguration<CryptoPriceEntity>
    {
        public void Configure(EntityTypeBuilder<CryptoPriceEntity> builder)
        {
            builder.HasNoKey();

            builder.HasOne(i => i.CryptoEntity);

            builder.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();
            
            
            builder.ToTable(DbTables.CryptoPriceTable.Name);
        }
    }
}
