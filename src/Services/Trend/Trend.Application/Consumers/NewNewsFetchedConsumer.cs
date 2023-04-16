using Events.Common.Trend;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Consumers
{
    public class NewNewsFetchedConsumer : IConsumer<NewNewsFetched>
    {
        private readonly ILogger<NewNewsFetchedConsumer> _logger;

        public NewNewsFetchedConsumer(ILogger<NewNewsFetchedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<NewNewsFetched> context)
        {
            _logger.LogInformation("New sync executed successfully");
            return Task.CompletedTask;
        }
    }
}
