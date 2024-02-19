using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Entities
{
    public sealed class Crypto : Entity
    {
        public string Symbol { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Logo { get; set; } = default!;
        public string WebSite { get; set; } = default!;
        public string SourceCode { get; set; } = default!;
        public ICollection<CryptoPrice> Prices { get; set; }
        public ICollection<Visit> Visits { get; set; }

        public Crypto()
        {
            Prices = new HashSet<CryptoPrice>();
            Visits = new HashSet<Visit>();
        }
    }
}
