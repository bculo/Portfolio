using Crypto.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastracture.Persistence.Configurations
{
    public class CryptoExplorerConfiguration : IEntityTypeConfiguration<CryptoExplorer>
    {
        public void Configure(EntityTypeBuilder<CryptoExplorer> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Url)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasOne(i => i.Crypto)
                .WithMany(i => i.Explorers)
                .HasForeignKey(i => i.CryptoId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
