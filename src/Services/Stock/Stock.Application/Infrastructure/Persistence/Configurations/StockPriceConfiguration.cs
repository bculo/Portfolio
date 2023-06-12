using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Application.Infrastructure.Persistence.Constants;
using Stock.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application.Infrastructure.Persistence.Configurations
{
    public class StockPriceConfiguration : IEntityTypeConfiguration<StockPrice>
    {
        public void Configure(EntityTypeBuilder<StockPrice> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasOne(i => i.Stock)
                .WithMany(i => i.Prices)
                .HasForeignKey(i => i.StockId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.ToTable(nameof(StockPrice), SchemaConstants.STOCK_SCHEMA);
        }
    }
}
