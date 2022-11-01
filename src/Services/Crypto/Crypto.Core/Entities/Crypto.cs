using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Entities
{
    public class Crypto : Entity
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string? WebSite { get; set; }
        public string? SourceCode { get; set; }
        public virtual ICollection<CryptoPrice> Prices { get; set; }
        public virtual ICollection<CryptoExplorer> Explorers { get; set; }
        public virtual ICollection<Visit> Visits { get; set; }

        public Crypto()
        {
            Prices = new HashSet<CryptoPrice>();
            Explorers = new HashSet<CryptoExplorer>();
            Visits = new HashSet<Visit>();
        }
    }
}
