using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Entities
{
    public class Crypto : AuditableEntity<long>
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public bool UpdateAutomatically { get; set; }
        public virtual ICollection<CryptoPrice> Prices { get; set; }

        public Crypto()
        {
            Prices = new HashSet<CryptoPrice>();
        }
    }
}
