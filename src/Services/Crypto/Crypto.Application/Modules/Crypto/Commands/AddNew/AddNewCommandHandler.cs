using Crypto.Application.Interfaces.Information;
using Crypto.Application.Interfaces.Information.Models;
using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Price.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using Crypto.Core.Exceptions;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Time.Abstract.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.AddNew
{
    public class AddNewCommandHandler : IRequestHandler<AddNewCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly ICryptoInfoService _infoService;
        private readonly ICryptoPriceService _priceService;
        private readonly IPublishEndpoint _publish;
        private readonly IDateTimeProvider _timeProvider;
        
        public AddNewCommandHandler(IUnitOfWork work,
            IPublishEndpoint publish, 
            ICryptoPriceService priceService, 
            ICryptoInfoService infoService, 
            IDateTimeProvider timeProvider)
        {
            _work = work;
            _publish = publish;
            _priceService = priceService;
            _infoService = infoService;
            _timeProvider = timeProvider;
        }

        public async Task Handle(AddNewCommand request, CancellationToken ct)
        {
            var item = await _work.CryptoRepo.First(
                i => EF.Functions.ILike(i.Symbol, request.Symbol),
                ct: ct);

            if(item != null)
            {
                throw new CryptoCoreException($"Item with given symbol {request.Symbol} already exist");
            }

            var cryptoInfoTask = _infoService.GetInformation(request.Symbol, ct);
            var cryptoPriceTask = _priceService.GetPriceInfo(request.Symbol, ct);

            await Task.WhenAll(cryptoInfoTask, cryptoPriceTask);
            
            var newCrypto = CreateNewCryptoEntity(cryptoInfoTask.Result);
            await _work.CryptoRepo.Add(newCrypto, ct);
            await _work.Commit(ct);
            
            var newPrice = CreateNewCryptoPriceEntity(cryptoPriceTask.Result, newCrypto.Id);
            await _work.CryptoPriceRepo.Add(newPrice, ct);
            await _work.Commit(ct);

            await _publish.Publish(new NewItemAdded
            {
                Id = newCrypto.Id,
                Name = newCrypto.Name,
                Price = newPrice!.Price,
                Symbol = newCrypto.Symbol,
                CorrelationId = request.CorrelationId ?? Guid.NewGuid()
            }, ct);
            
            await _publish.Publish(new EvictRedisListRequest(), ct);
        }

        private CryptoPrice CreateNewCryptoPriceEntity(PriceResponse result, Guid cryptoId)
        {
            return new CryptoPrice
            {
                CryptoId = cryptoId,
                Price = result.Price,
                Time = _timeProvider.UtcOffset
            };
        }

        private Core.Entities.Crypto CreateNewCryptoEntity(CryptoInformation info)
        {
            return new Core.Entities.Crypto
            {
                Id = Guid.NewGuid(),
                Logo = info.Logo,
                Name = info.Name,
                Symbol = info.Symbol,
                Description = info.Description,
                WebSite = info.Website,
                SourceCode = info.SourceCode
            };
        }
    }
}
