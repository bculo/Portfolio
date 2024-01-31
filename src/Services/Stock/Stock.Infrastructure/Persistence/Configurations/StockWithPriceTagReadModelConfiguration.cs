using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Core.Models.Stock;

namespace Stock.Infrastructure.Persistence.Configurations
{
    public class StockWithPriceTagReadModelConfiguration : IEntityTypeConfiguration<StockWithPriceTag>
    {
        public void Configure(EntityTypeBuilder<StockWithPriceTag> builder)
        {
            builder.ToView("stock_with_price_tag")
                .HasNoKey();
        }
    }
}
