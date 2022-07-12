using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Models.Price
{
    public class CryptoPriceResponseDto
    {
        public string Symbol { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
    }
}
