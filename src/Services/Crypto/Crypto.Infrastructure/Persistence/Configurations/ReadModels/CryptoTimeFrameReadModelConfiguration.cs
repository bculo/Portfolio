using Crypto.Core.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crypto.Infrastructure.Persistence.Configurations.ReadModels;

public class CryptoTimeFrameReadModelConfiguration : IEntityTypeConfiguration<CryptoTimeFrameReadModel>
{
    public void Configure(EntityTypeBuilder<CryptoTimeFrameReadModel> builder)
    {
        builder.HasNoKey();
        builder.ToTable(j => j.ExcludeFromMigrations());
    }
}