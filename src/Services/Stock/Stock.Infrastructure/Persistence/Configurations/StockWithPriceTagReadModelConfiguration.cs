using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Core.Models.Stock;

namespace Stock.Infrastructure.Persistence.Configurations
{
    public class StockWithPriceTagReadModelConfiguration : IEntityTypeConfiguration<StockWithPriceTagReadModel>
    {
        public void Configure(EntityTypeBuilder<StockWithPriceTagReadModel> builder)
        {
            builder.ToView("stock_with_price_tag")
                .HasNoKey();
        }
    }
}
