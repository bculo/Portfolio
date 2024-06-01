
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;

namespace Trend.Application.Services
{
    public class MongoTransactionService(
        IClientSessionHandle session,
        IOptions<MongoOptions> options)
        : ITransaction
    {
        private readonly MongoOptions _options = options.Value;

        public Task AbortTransaction(CancellationToken token = default)
        {
            if (_options.ServerType == ServerType.Replica && session.IsInTransaction)
            {
                session.AbortTransactionAsync(token);
            }

            return Task.CompletedTask;
        }

        public Task CommitTransaction(CancellationToken token = default)
        {
            if (_options.ServerType == ServerType.Replica && session.IsInTransaction)
            {
                session.CommitTransactionAsync(token);
            }

            return Task.CompletedTask;
        }
        

        public Task StartTransaction(CancellationToken token = default)
        {
            if (_options.ServerType == ServerType.Replica && !session.IsInTransaction)
            {
                session.StartTransaction();
            }

            return Task.CompletedTask;
        }
    }
}
