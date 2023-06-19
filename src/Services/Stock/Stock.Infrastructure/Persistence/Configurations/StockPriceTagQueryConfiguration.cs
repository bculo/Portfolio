using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Core.Queries;
using Stock.Infrastructure.Persistence.Constants;

namespace Stock.Infrastructure.Persistence.Configurations
{
    public class StockPriceTagQueryConfiguration : IEntityTypeConfiguration<StockPriceTagQuery>
    {
        public void Configure(EntityTypeBuilder<StockPriceTagQuery> builder)
        {
            builder.ToView("stockwithpricetag", SchemaConstants.STOCK_SCHEMA).HasNoKey();
        }
    }
}
