using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchAll
{
    public class FetchAllResponseDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string SourceCode { get; set; }
        public DateTime Created { get; set; }
        public string Logo { get; set; }
        public decimal Price { get; set; }
    }
}
