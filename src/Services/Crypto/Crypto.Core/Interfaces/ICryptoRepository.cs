using Crypto.Core.Entities;
using Crypto.Core.Queries.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Interfaces
{
    public interface ICryptoRepository : IRepository<Core.Entities.Crypto>
    {
        Task<CryptoLastPrice> GetWithPrice(string symbol);
        Task<List<CryptoLastPrice>> GetAllWithPrice();
        Task<Dictionary<string, Entities.Crypto>> GetAllAsDictionary();
        Task<List<CryptoMostPopularQuery>> GetMostPopular(int take);
        Task<List<CryptoLastPrice>> GetPageWithPrices(int page, int take);
        Task<List<CryptoLastPrice>> GetGroupWithPrices(List<string> symbols, int page, int take);
        Task<List<CryptoLastPrice>> SearchBySymbol(string symbol, int page, int take);
        Task<List<string>> GetAllSymbols();
    }
}
