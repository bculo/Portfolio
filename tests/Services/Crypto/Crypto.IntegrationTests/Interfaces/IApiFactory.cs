using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.Interfaces
{
    public interface IApiFactory
    {
        HttpClient Client { get; }
        Task ResetDatabaseAsync();
    }
}
