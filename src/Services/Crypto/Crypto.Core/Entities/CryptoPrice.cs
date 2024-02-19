using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Entities
{
    public class CryptoPrice
    {
        public DateTimeOffset Time { get; set; }
        public decimal Price { get; set; }
        public Guid CryptoId { get; set; }
        public virtual Crypto? Crypto { get; set; }
    }
}
