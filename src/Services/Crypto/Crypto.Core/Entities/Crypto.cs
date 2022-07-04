using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Entities
{
    public class Crypto : Entity<long>
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public virtual List<CryptoPrice> Prices { get; set; }
    }
}
