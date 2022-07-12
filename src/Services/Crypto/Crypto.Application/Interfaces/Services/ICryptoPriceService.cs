using Crypto.Application.Models.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Interfaces.Services
{
    public interface ICryptoPriceService
    {
        Task<CryptoPriceResponseDto> GetPriceInfo(string symbol);
        Task<List<CryptoPriceResponseDto>> GetPriceInfo(List<string> symbols);
    }
}
