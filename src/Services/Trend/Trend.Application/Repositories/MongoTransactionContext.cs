using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trend.Application.Configurations.Persistence;
using Trend.Application.Interfaces.Repositories;
using Trend.Application.Options;

namespace Trend.Application.Repositories
{
    public class MongoTransactionContext : IMongoContext
    {
        private IMongoDatabase Database { get; set; }
        private IClientSessionHandle Session { get; set; }
        private MongoClient MongoClient { get; set; }

        private readonly MongoOptions _options;
        private readonly List<Func<Task>> _commands;

        public MongoTransactionContext(IOptions<MongoOptions> options)
        {
            _commands = new List<Func<Task>>();
            _options = options.Value;

            MongoConfiguration.Configure();

            MongoClient = new MongoClient(_options.ConnectionString);
            Database = MongoClient.GetDatabase(_options.DatabaseName);
        }


        public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName)
        {
            return Database.GetCollection<TDocument>(collectionName);
        }

        public void AddCommand(Func<Task> command)
        {
            if(command is null)
            {
                return;
            }

            _commands.Add(command);
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                using (Session = await MongoClient.StartSessionAsync())
                {
                    Session.StartTransaction();

                    var commandTasks = _commands.Select(c => c());

                    await Task.WhenAll(commandTasks);

                    await Session.CommitTransactionAsync();
                }
            }
            catch(Exception e)
            {
                await Session.AbortTransactionAsync();
                throw;
            }

            return _commands.Count;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
