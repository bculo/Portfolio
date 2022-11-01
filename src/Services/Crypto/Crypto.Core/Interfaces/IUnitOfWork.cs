using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Interfaces
{
    public interface IUnitOfWork
    {
        ICryptoPriceRepository CryptoPriceRepository { get; }
        ICryptoRepository CryptoRepository { get; }
        ICryptoExplorerRepository CryptoExplorerRepository { get; }
        IVisitRepository VisitRepository { get; }

        Task Commit();
    }
}
