using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Core.Entities;
using Stock.Infrastructure.Persistence.Constants;

namespace Stock.Infrastructure.Persistence.Configurations
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

            builder.ToTable(nameof(StockPrice).ToLower(), SchemaConstants.STOCK_SCHEMA);
        }
    }
}
