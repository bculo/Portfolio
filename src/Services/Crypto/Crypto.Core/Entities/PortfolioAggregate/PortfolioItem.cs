using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Entities.PortfolioAggregate
{
    public class PortfolioItem : Entity
    {
        public string Symbol { get; set; }
        public int Quantity { get; set; }
        public decimal AvaragePrice { get; set; }
        public long PortfolioId { get; set; }
        public virtual Portfolio? Portfolio { get; set; }
    }
}
