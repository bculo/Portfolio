
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
        private readonly ILogger<MongoTransactionService> _logger;
        private readonly MongoOptions _options;

        public MongoTransactionService(IClientSessionHandle session,
            ILogger<MongoTransactionService> logger,
            IOptions<MongoOptions> options)
        {
            _session = session;
            _logger = logger;
            _options = options.Value;
        }

        public Task AbortTransaction()
        {
            try
            {
                if (_options.ServerType == ServerType.Replica && _session.IsInTransaction)
                {
                    _session.AbortTransaction();
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return Task.CompletedTask;
        }

        public Task CommitTransaction()
        {
            if (_options.ServerType == ServerType.Replica)
            {
                _session.CommitTransaction();
            }

            return Task.CompletedTask;
        }

        public Task StartTransaction()
        {
            if (_options.ServerType == ServerType.Replica)
            {
                _session.StartTransaction();
            }

            return Task.CompletedTask;
        }
    }
}
