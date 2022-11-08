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
        public string? Name { get; set; }
        public PortfolioStatus Status { get; set; }
        public virtual ICollection<PortfolioItem> Items { get; set; }
        
        public Portfolio()
        {
            Items = new HashSet<PortfolioItem>();
        }
    }
}
