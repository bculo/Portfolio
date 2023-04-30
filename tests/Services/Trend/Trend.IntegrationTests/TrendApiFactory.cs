using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Trend.IntegrationTests
{
    public interface IApiFactory
    {
        HttpClient Client { get; }
    }

    public class TrendApiFactory : WebApplicationFactory<Program>, IAsyncLifetime, IApiFactory
    {
        public HttpClient Client { get; private set; }

        public TrendApiFactory()
        {

        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
        }

        public async Task InitializeAsync()
        {

            Client = CreateClient();
        }

        public async new Task DisposeAsync()
        {
        }       
    }
}
