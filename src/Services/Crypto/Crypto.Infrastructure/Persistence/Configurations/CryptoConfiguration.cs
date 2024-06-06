using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crypto.Infrastructure.Persistence.Configurations
{
    public class CryptoConfiguration : IEntityTypeConfiguration<Core.Entities.Crypto>
    {
        public void Configure(EntityTypeBuilder<Core.Entities.Crypto> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasIndex(i => i.Symbol)
                .IsUnique();

            builder.Property(i => i.Symbol)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(i => i.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(i => i.WebSite)
                .HasMaxLength(250)
                .IsRequired(false);

            builder.Property(i => i.SourceCode)
                .HasMaxLength(250)
                .IsRequired(false);

            builder.ToTable("crypto");
        }
    }
}
