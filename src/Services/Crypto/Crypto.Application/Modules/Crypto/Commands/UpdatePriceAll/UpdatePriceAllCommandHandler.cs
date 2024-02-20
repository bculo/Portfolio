using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Repositories;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll
{
    public class UpdatePriceAllCommandHandler : IRequestHandler<UpdatePriceAllCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly ICryptoPriceService _priceService;
        private readonly IPublishEndpoint _publish;
        private readonly ILogger<UpdatePriceAllCommandHandler> _logger;

        public UpdatePriceAllCommandHandler(IUnitOfWork work,
            ICryptoPriceService priceService,
            IPublishEndpoint publish,
            ILogger<UpdatePriceAllCommandHandler> logger)
        {
            _work = work;
            _priceService = priceService;
            _publish = publish;
            _logger = logger;
        }

        public async Task Handle(UpdatePriceAllCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // var entityDict = await _work.CryptoRepository.GetAllAsDictionary();
            //
            // if (!entityDict.Any())
            // {
            //     _logger.LogTrace("ZERO entities to update");
            //     return;
            // }
            //
            // var symbols = entityDict.Keys.ToList();
            //
            // var response = await _priceService.GetPriceInfo(symbols!);
            // CryptoCoreException.ThrowIfNull(response, "Unexcpected error");
            //
            // var newPrices = new List<CryptoPrice>();
            // var events = new List<CryptoPriceUpdated>();
            //
            // foreach(var item in response)
            // {
            //     if(!entityDict.ContainsKey(item.Symbol!.ToUpper()))
            //     {
            //         continue;
            //     }
            //
            //     var crypto = entityDict[item.Symbol!.ToUpper()];
            //
            //     /*
            //     newPrices.Add(new CryptoPrice
            //     {
            //         CryptoId = crypto.Id,
            //         Price = item.Price
            //     });
            //     */
            //
            //     events.Add(new CryptoPriceUpdated
            //     {
            //         Id = crypto.Id,
            //         Currency = item.Currency,
            //         Name = crypto.Name,
            //         Price = item.Price,
            //         Symbol = item.Symbol
            //     });
            // }
            //
            // await PublishEvents(events);
            //
            // await _work.Commit();
        }

        private async Task PublishEvents(List<CryptoPriceUpdated> events)
        {
            foreach (var item in events)
            {
                await _publish.Publish(item);
            }
        }
    }
}
