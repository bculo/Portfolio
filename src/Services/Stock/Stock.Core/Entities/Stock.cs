using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Entities
{
    public class Stock : AuditableEntity
    {
        public int Id { get; set; }
        public string Symbol { get; set; } 
        public virtual ICollection<StockPrice> Prices { get; set; }

        public Stock()
        {
            Prices = new HashSet<StockPrice>();
        }
    }
}
