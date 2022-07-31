using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.Interfaces
{
    public interface IWebHostHelpBuilder
    {
        void ConfigureServices(Action<IServiceCollection> action);
    }
}
