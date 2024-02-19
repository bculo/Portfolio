﻿using Crypto.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crypto.Infrastructure.Persistence.Configurations
{
    public class CryptoLastPriceConfiguration : IEntityTypeConfiguration<CryptoLastPrice>
    {
        public void Configure(EntityTypeBuilder<CryptoLastPrice> builder)
        {
            builder.ToView("CryptoLastPrice", "dbo").HasNoKey();
        }
    }
}