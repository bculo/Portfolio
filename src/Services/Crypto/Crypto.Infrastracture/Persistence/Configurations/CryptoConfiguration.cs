using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastracture.Persistence.Configurations
{
    public class CryptoConfiguration : IEntityTypeConfiguration<Core.Entities.Crypto>
    {
        public void Configure(EntityTypeBuilder<Core.Entities.Crypto> builder)
        {
            
        }
    }
}
