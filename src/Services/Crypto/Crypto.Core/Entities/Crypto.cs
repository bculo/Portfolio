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
        public ICollection<Visit> Visits { get; set; }

        public Crypto()
        {
            Visits = new HashSet<Visit>();
        }
    }
}
