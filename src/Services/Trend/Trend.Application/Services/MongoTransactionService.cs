
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;

namespace Trend.Application.Services
{
    public class MongoTransactionService : ITransaction
    {
        private readonly IClientSessionHandle _session;
        private readonly MongoOptions _options;

        public MongoTransactionService(IClientSessionHandle session,
            IOptions<MongoOptions> options)
        {
            _session = session;
            _options = options.Value;
        }

        public Task AbortTransaction(CancellationToken token = default)
        {
            if (_options.ServerType == ServerType.Replica && _session.IsInTransaction)
            {
                _session.AbortTransactionAsync(token);
            }

            return Task.CompletedTask;
        }

        public Task CommitTransaction(CancellationToken token = default)
        {
            if (_options.ServerType == ServerType.Replica && _session.IsInTransaction)
            {
                _session.CommitTransactionAsync(token);
            }

            return Task.CompletedTask;
        }
        

        public Task StartTransaction(CancellationToken token = default)
        {
            if (_options.ServerType == ServerType.Replica && !_session.IsInTransaction)
            {
                _session.StartTransaction();
            }

            return Task.CompletedTask;
        }
    }
}
