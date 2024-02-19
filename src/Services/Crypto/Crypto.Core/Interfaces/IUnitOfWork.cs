using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Interfaces
{
    public interface IUnitOfWork
    {
        ICryptoRepository CryptoRepository { get; }
        IVisitRepository VisitRepository { get; }
        Task Commit();
    }
}
