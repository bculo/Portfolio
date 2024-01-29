using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Core.Models.Stock;

namespace Stock.Infrastructure.Persistence.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<StockEntity>
    {
        public void Configure(EntityTypeBuilder<StockEntity> builder)
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

            builder.ToTable("stocks");
        }
    }
}
