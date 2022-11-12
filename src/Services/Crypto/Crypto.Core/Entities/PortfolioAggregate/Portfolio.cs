using Crypto.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Entities.PortfolioAggregate
{
    public class Portfolio : Entity, IAggregateRoot
    {
        public string? Name { get; private set; }
        public PortfolioStatus Status { get; private set; }
        public virtual ICollection<PortfolioItem> Items { get; set; }
        
        public Portfolio(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Status = PortfolioStatus.ACTIVE;

            Items = new HashSet<PortfolioItem>();
        }
    }
}
