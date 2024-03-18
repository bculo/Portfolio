using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPage
{
    public class FetchPageResponseDto
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Website { get; set; } = default!;
        public string SourceCode { get; set; } = default!;
        public decimal Price { get; set; } = default!;
    }
}
