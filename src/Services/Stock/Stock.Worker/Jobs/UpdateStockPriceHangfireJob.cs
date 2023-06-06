using Stock.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Worker.Jobs
{
    public interface IPriceUpdateJobService
    {
        Task InitializeUpdateProcedure();
    }

    public class UpdateStockPriceHangfireJob : IPriceUpdateJobService
    {
        private readonly ILogger<UpdateStockPriceHangfireJob> _logger;
        private readonly IStockPriceClient _client;

        public UpdateStockPriceHangfireJob(IStockPriceClient client, 
            ILogger<UpdateStockPriceHangfireJob> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task InitializeUpdateProcedure()
        {
            var result = await _client.GetPriceForSymbol("TSLA");
        }
    }
}
