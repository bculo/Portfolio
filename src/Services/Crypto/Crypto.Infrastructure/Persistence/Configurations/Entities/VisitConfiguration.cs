using Crypto.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crypto.Infrastructure.Persistence.Configurations.Entities
{
    public class VisitConfiguration : IEntityTypeConfiguration<VisitEntity>
    {
        public void Configure(EntityTypeBuilder<VisitEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.Crypto)
                .WithMany(i => i.Visits)
                .HasForeignKey(i => i.CryptoId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.ToTable(DbTables.VisitTable.Name);
        }
    }
}
