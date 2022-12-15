using Crypto.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastracture.Persistence.Configurations
{
    public class PortfolioItemConfiguration : IEntityTypeConfiguration<PortfolioItem>
    {
        public void Configure(EntityTypeBuilder<PortfolioItem> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.AvaragePrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasOne(i => i.Portfolio)
                .WithMany(i => i.Items)
                .HasForeignKey(i => i.PortfolioId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(i => i.Crypto)
                .WithMany(i => i.PortfolioItems)
                .HasForeignKey(i => i.CryptoId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
