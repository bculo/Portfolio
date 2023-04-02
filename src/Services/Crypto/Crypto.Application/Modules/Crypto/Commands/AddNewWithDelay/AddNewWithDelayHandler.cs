using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewWithDelay
{
    public class AddNewWithDelayHandler : IRequestHandler<AddNewWithDelayCommand, Guid>
    {
        private readonly ILogger<AddNewWithDelayHandler> _logger;
        private readonly IPublishEndpoint _publish;

        public AddNewWithDelayHandler(ILogger<AddNewWithDelayHandler> logger,
            IPublishEndpoint publish)
        {
            _logger = logger;
            _publish = publish;
        }

        public async Task<Guid> Handle(AddNewWithDelayCommand request, CancellationToken cancellationToken)
        {
            var temporaryID = Guid.NewGuid();

            await _publish.Publish(new AddCryptoItemWithDelay
            {
                Symbol = request.Symbol,
                TemporaryId = temporaryID
            }, cancellationToken);

            return temporaryID;
        }
    }
}
