using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Models.Price
{
    public class CryptoPriceSingleResponseDto
    {
        public string Symbol { get; set; }
        public List<string> Currencies { get; set; }
        public Dictionary<string, decimal>  Prices { get; set; }
        public bool ValidResponse { get; set; }
    }
}
