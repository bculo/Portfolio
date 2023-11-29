using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Options
{
    public sealed class MongoOptions
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public bool UseInterceptor { get; set; }
        public int ConnectionTimeoutSeconds { get; set; }
        public ServerType ServerType { get; set; }
        public MongoInterceptorSettings InterceptorSettings { get; set; }

    }

    public sealed class MongoInterceptorSettings
    {
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string AuthMechanisam { get; set; }
        public string AuthDatabase { get; set; }
    }

    public enum ServerType
    {
        Standalone = 0,
        Replica = 1
    }

}
