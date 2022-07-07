using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Entities
{
    public class CryptoExplorer : Entity
    {
        public string Url { get; set; }
        public long CryptoId { get; set; }
        public virtual Crypto Crypto { get; set; }
    }
}
