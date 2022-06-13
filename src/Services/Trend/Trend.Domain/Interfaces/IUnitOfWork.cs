using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Entities;

namespace Trend.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<SyncStatus> Syncs { get; }
        IRepository<Article> Articles { get; }

        Task<bool> CommitAsync();
    }
}
