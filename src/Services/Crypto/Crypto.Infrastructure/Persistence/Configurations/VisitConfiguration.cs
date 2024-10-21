using Crypto.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crypto.Infrastructure.Persistence.Configurations
{
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public const string TableName = "visit";
        
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.Crypto)
                .WithMany(i => i.Visits)
                .HasForeignKey(i => i.CryptoId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.ToTable(TableName);
        }
    }
}
