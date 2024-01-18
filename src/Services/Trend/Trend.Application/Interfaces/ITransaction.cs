using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces
{
    public interface ITransaction
    {
        Task StartTransaction(CancellationToken token = default);
        Task AbortTransaction(CancellationToken token = default);
        Task CommitTransaction(CancellationToken token = default);
    }
}
