using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crypto.Infrastructure.Persistence.Configurations.Entities
{
    public class CryptoConfiguration : IEntityTypeConfiguration<Core.Entities.CryptoEntity>
    {
        public void Configure(EntityTypeBuilder<Core.Entities.CryptoEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasIndex(i => i.Symbol)
                .IsUnique();

            builder.Property(i => i.Symbol)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Property(x => x.Description)
                .IsRequired(false);

            builder.Property(i => i.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(i => i.WebSite)
                .HasMaxLength(250)
                .IsRequired(false);

            builder.Property(i => i.SourceCode)
                .HasMaxLength(250)
                .IsRequired(false);

            builder.ToTable(DbTables.CryptoTable.Name);
        }
    }
}
