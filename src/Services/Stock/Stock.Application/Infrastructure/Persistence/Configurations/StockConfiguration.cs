using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Application.Infrastructure.Persistence.Constants;

namespace Stock.Application.Infrastructure.Persistence.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<Core.Entities.Stock>
    {
        public void Configure(EntityTypeBuilder<Core.Entities.Stock> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(i => i.Symbol)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(i => i.CreatedBy) 
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(i => i.ModifiedBy)
                .HasMaxLength(100)
                .IsRequired();

            builder.ToTable(nameof(Core.Entities.Stock), SchemaConstants.STOCK_SCHEMA);
        }
    }
}
