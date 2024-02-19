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
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Price)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(i => i.Crypto)
                .WithMany(i => i.Prices)
                .HasForeignKey(i => i.CryptoId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
        }
    }
}
