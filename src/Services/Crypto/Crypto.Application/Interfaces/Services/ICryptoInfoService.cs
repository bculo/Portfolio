using Crypto.Application.Models.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Interfaces.Services
{
    public interface ICryptoInfoService
    {
        Task<CryptoInfoResponseDto> FetchData(string symbol);
    }
}
