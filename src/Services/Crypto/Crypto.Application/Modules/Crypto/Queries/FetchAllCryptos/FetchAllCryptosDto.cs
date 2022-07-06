using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchAllCryptos
{
    public class FetchAllCryptosDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }
}
