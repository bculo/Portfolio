using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Entities
{
    public class CryptoPrice : Entity<long>
    {
        public decimal Price { get; set; }
        public long CryptoId { get; set; }
        public virtual Crypto Crypto { get; set; }
    }
}
