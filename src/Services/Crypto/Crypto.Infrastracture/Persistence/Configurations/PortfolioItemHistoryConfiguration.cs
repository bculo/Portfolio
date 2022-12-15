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
    public class PortfolioItemHistoryConfiguration : IEntityTypeConfiguration<PortfolioItemHistory>
    {
        public void Configure(EntityTypeBuilder<PortfolioItemHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(i => i.Quantity)
                .IsRequired();

            builder.Property(i => i.Price)
                .HasPrecision(2)
                .IsRequired();

            builder.HasOne(i => i.PortfolioItem)
                .WithMany(v => v.History)
                .HasForeignKey(i => i.PortfolioItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
