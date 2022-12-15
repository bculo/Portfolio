using Crypto.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Entities
{
    public class Portfolio : Entity
    {
        public string? Name { get; set; }
        public PortfolioStatus Status { get; set; }
        public virtual ICollection<PortfolioItem> Items { get; set; }

        public Portfolio(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Status = PortfolioStatus.ACTIVE;
            Items = new HashSet<PortfolioItem>();
        }
    }
}
