using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Configurations.Options
{
    public sealed class MongoOptions
    {
        public string ConnectionString { get; set; } = default!;
        public string DatabaseName { get; set; } = default!;
        public bool UseInterceptor { get; set; }
        public int ConnectionTimeoutSeconds { get; set; }
        public ServerType ServerType { get; set; }
        public MongoInterceptorSettings InterceptorSettings { get; set; } = default!;

    }

    public sealed class MongoInterceptorSettings
    {
        public string Host { get; set; } = default!;
        public string User { get; set; } = default!;
        public string Password { get; set; } = default!;
        public int Port { get; set; }
        public string AuthMechanisam { get; set; } = default!;
        public string AuthDatabase { get; set; } = default!;
    }

    public enum ServerType
    {
        Standalone = 0,
        Replica = 1
    }

}
