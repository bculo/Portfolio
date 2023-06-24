using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Entities;
using User.Application.Persistence.Constants;

namespace User.Application.Persistence.Configurations
{
    public class PortfolioUserConfiguration : IEntityTypeConfiguration<PortfolioUser>
    {
        public void Configure(EntityTypeBuilder<PortfolioUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(i => i.UserName)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.UserName)
                .IsUnique();

            builder.Property(i => i.FirstName)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(i => i.LastName)
                .HasMaxLength(128)
                .IsRequired();

            builder.ToTable("portfoliousers", SchemaConstants.BASE_SCHEMA);
        }
    }
}
