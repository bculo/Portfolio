using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Interfaces.Repositories;
using Trend.Domain.Entities;
using Trend.Domain.Interfaces;

namespace Trend.Application.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _context;

        public IRepository<SyncStatus> Syncs { get; private set; }
        public IRepository<Article> Articles { get; private set; }

        public UnitOfWork(IMongoContext context)
        {
            _context = context;

            Syncs = new MongoTransactionRepository<SyncStatus>(context);
            Articles = new MongoTransactionRepository<Article>(context);
        }

        public async Task<bool> CommitAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
