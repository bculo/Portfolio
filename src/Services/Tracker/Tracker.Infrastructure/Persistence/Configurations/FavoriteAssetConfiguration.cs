using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracker.Core.Entities;

namespace Tracker.Infrastructure.Persistence.Configurations
{
    public class FavoriteAssetConfiguration : IEntityTypeConfiguration<FavoriteAsset>
    {
        public void Configure(EntityTypeBuilder<FavoriteAsset> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(p => p.Symbol)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(p => p.UserId)
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(p => p.AssetType)
                .IsRequired();
        }
    }
}
