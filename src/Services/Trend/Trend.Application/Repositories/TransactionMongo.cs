using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Interfaces;
using Trend.Application.Options;

namespace Trend.Application.Repositories
{
    public class TransactionMongo : ITransaction
    {
        private readonly MongoOptions _options;

        private IClientSessionHandle _session;

        public TransactionMongo(IOptions<MongoOptions> options)
        {
            _options = options.Value;
        }

        public void Commit()
        {
            try
            {
                _session.CommitTransaction();
                _session?.Dispose();
            }
            catch
            {
                Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            try
            {
                _session.AbortTransaction();
                _session?.Dispose();
            }
            catch
            {
                throw;
            }
        }

        public void Start()
        {
            var client = new MongoClient(_options.ConnectionString);

            _session = client.StartSession();
            _session.StartTransaction();
        }
    }
}
