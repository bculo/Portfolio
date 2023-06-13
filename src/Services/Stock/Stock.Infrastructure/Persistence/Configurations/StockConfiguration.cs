using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.Infrastructure.Persistence.Constants;

namespace Stock.Infrastructure.Persistence.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<Core.Entities.Stock>
    {
        public void Configure(EntityTypeBuilder<Core.Entities.Stock> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Symbol)
                .IsUnique();

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
