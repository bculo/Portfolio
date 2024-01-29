using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Core.Models;
using Stock.Core.Models.Stock;

namespace Stock.Infrastructure.Persistence.Configurations
{
    public class StockPriceConfiguration : IEntityTypeConfiguration<StockPriceEntity>
    {
        public void Configure(EntityTypeBuilder<StockPriceEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasOne(i => i.StockEntity)
                .WithMany(i => i.Prices)
                .HasForeignKey(i => i.StockId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.ToTable("stocks_prices");
        }
    }
}
