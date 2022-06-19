using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Options;

namespace Trend.Application.Utils.Persistence
{
    public static class TrendMongoUtils
    {
        public static MongoClient CreateMongoClient(MongoOptions options)
        {
            if(!options.UseInterceptor)
            {
                var client = new MongoClient(options.ConnectionString);
                client.Settings.ConnectTimeout = TimeSpan.FromSeconds(options.ConnectionTimeoutSeconds);
                return client;
            }

            var mongoIdentity = new MongoInternalIdentity(options.InterceptorSettings.AuthDatabase, options.InterceptorSettings.User);
            var mongoPassword = new PasswordEvidence(options.InterceptorSettings.Password);

            var credential = new MongoCredential(options.InterceptorSettings.AuthMechanisam, mongoIdentity, mongoPassword);
            var server = new MongoServerAddress(options.InterceptorSettings.Host, options.InterceptorSettings.Port);

            return new MongoClient(new MongoClientSettings
            {
                UseTls = false,
                Server = server,
                Credential = credential,
                ConnectTimeout = TimeSpan.FromSeconds(options.ConnectionTimeoutSeconds),
                ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(e =>
                    {
                        Debug.WriteLine($"{e.CommandName} - {e.Command.ToJson(new JsonWriterSettings { Indent = true })}");
                        Debug.WriteLine(new String('-', 32));
                    });
                }
            });
        }
    }
}
