using Crypto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Interfaces
{
    public interface IPortfolioRepositry
    {
        Task Add(Portfolio p); 
        Task<IEnumerable<Portfolio>> GetPortfolios(string userId);
        Task<Portfolio> GetPortfolio(string name, string userId);
    }
}
