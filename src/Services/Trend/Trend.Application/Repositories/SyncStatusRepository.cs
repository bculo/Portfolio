using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Options;
using Trend.Domain.Entities;
using Trend.Domain.Interfaces;

namespace Trend.Application.Repositories
{
    public class SyncStatusRepository : MongoRepository<SyncStatus>, ISyncStatusRepository
    {
        public SyncStatusRepository(IOptions<MongoOptions> options) : base(options)
        {
                
        }

        public virtual async Task<SyncStatus> GetLastValidSync()
        {
            return _collection.Find(t => t.TotalRequests > 0 && t.SucceddedRequests > 0)
                            .SortByDescending(i => i.Created)
                            .FirstOrDefault();
        }
    }
}
