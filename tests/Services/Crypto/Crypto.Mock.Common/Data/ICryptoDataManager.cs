using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Mock.Common.Data
{
    public interface ICryptoDataManager
    {
        void InitData(List<Core.Entities.Crypto>? seedData, List<string>? supportedSymbols);
        List<Core.Entities.Crypto> GetCryptoSeedData();
        List<string> GetSupportedCryptoSymbols();
        string[] GetSupportedCryptoSymbolsArray();
        bool IsSymbolSupported(string symbol);
        bool IsAnySymbolSupported(IEnumerable<string> symbols);
    }
}
