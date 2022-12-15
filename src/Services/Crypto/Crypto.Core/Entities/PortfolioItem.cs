using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Entities
{
    public class PortfolioItem : Entity
    {
        public int Quantity { get; set; }
        public decimal AvaragePrice { get; set; }
        public long PortfolioId { get; set; }
        public virtual Portfolio? Portfolio { get; set; }
        public long CryptoId { get; set; }
        public virtual Crypto? Crypto { get; set; }
        public virtual ICollection<PortfolioItemHistory> History { get; set; }

        public PortfolioItem()
        {
            History = new HashSet<PortfolioItemHistory>();
        }
    }
}
