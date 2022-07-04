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
    public class CryptoPriceConfiguration : IEntityTypeConfiguration<Core.Entities.CryptoPrice>
    {
        public void Configure(EntityTypeBuilder<CryptoPrice> builder)
        {
            
        }
    }
}
