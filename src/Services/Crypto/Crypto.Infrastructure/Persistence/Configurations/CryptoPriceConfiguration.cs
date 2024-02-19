using Crypto.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastructure.Persistence.Configurations
{
    public class CryptoPriceConfiguration : IEntityTypeConfiguration<Core.Entities.CryptoPrice>
    {
        public void Configure(EntityTypeBuilder<CryptoPrice> builder)
        {
            builder.HasNoKey();

            builder.HasOne(i => i.Crypto);

            builder.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();
            
            builder.ToTable("crypto_price");
        }
    }
}
