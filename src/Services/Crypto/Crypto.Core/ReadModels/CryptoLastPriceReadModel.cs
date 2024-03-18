using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.ReadModels
{
    public class CryptoLastPriceReadModel
    {
        public Guid CryptoId { get; set; }
        public string Symbol { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Website { get; set; }
        public string? SourceCode { get; set; }
        public decimal LastPrice { get; set; }
    }
}
