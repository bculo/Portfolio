using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
using Crypto.Application.Models.Price;
using Crypto.Core.Entities;
using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Time.Common.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.AddNew
{
    public class AddNewCommandHandler : IRequestHandler<AddNewCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly ICryptoInfoService _infoService;
        private readonly ICryptoPriceService _priceService;
        private readonly ILogger<AddNewCommandHandler> _logger;
        private readonly IPublishEndpoint _publish;
        private readonly IDateTime _time;

        public CryptoInfoDataDto? Info { get; set; }
        public CryptoPriceResponseDto? Price { get; set; }

        public AddNewCommandHandler(IUnitOfWork work,
            ICryptoInfoService infoService,
            ICryptoPriceService priceService,
            ILogger<AddNewCommandHandler> logger,
            IPublishEndpoint publish,
            IDateTime time)
        {
            _work = work;
            _infoService = infoService;
            _priceService = priceService;
            _logger = logger;
            _publish = publish;
            _time = time;
        }

        public async Task<Unit> Handle(AddNewCommand request, CancellationToken cancellationToken)
        {
            var item = await _work.CryptoRepository.FindSingle(i => i.Symbol!.ToLower() == request.Symbol!.ToLower());

            if(item != null)
            {
                _logger.LogInformation("Item with given symbol {0} already exist", request.Symbol);
                throw new CryptoCoreException($"Item with given symbol {request.Symbol} already exist");
            }

            var cryptoInfoTask = _infoService.FetchData(request.Symbol);
            var cryptoPriceTask = _priceService.GetPriceInfo(request.Symbol);

            await Task.WhenAll(cryptoInfoTask, cryptoPriceTask);

            var info = cryptoInfoTask.Result;
            var price = cryptoPriceTask.Result;

            if(info is null || price is null)
            {
                _logger.LogInformation("Info and price items are not fetched successfuly (HTTP clients)");
                throw new CryptoCoreException("Provided symbol not supported");
            }

            ParseData(info, price, request.Symbol);

            var newInstance = CreateNewInstance();

            await _work.Commit();

            await _publish.Publish(new NewCryptoAdded
            {
                CreatedOn = _time.DateTime,
                Currency = Price!.Currency,
                Id = newInstance.Id,
                Name = newInstance.Name,
                Price = Price!.Price,
                Symbol = newInstance.Symbol,
            });

            return Unit.Value;
        }

        private Core.Entities.Crypto CreateNewInstance()
        {
            var newCrypto = new Core.Entities.Crypto
            {
                Logo = Info!.Logo,
                Name = Info!.Name,
                Symbol = Info!.Symbol,
                Description = Info!.Description,
                WebSite = Info.Urls.ContainsKey("website") ? Info!.Urls["website"]!.FirstOrDefault() : null,
                SourceCode = Info.Urls.ContainsKey("source_code") ? Info!.Urls["source_code"]!.FirstOrDefault() : null,
            };

            if (Info.Urls.ContainsKey("explorer"))
            {
                newCrypto.Explorers = Info!.Urls["explorer"]?.Select(i => new CryptoExplorer
                {
                    Url = i
                }).ToList() ?? new List<CryptoExplorer>();
            }

            newCrypto.Prices.Add(new CryptoPrice
            {
                Price = Price!.Price
            });
            
            return newCrypto;
        }

        private void ParseData(CryptoInfoResponseDto infoResponse, CryptoPriceResponseDto priceResponse, string symbol)
        {
            var cryptoData = infoResponse.Data.Values.FirstOrDefault();

            if (cryptoData is null || !cryptoData.Any())
            {
                _logger.LogInformation("Information about given symbol not found");
                throw new Exception("Unexpected exception");
            }

            Info = cryptoData.FirstOrDefault(i => i.Symbol.ToLower() == symbol.ToLower()
                                    && !string.IsNullOrEmpty(i.Description)
                                    && !string.IsNullOrEmpty(i.Name)
                                    && !string.IsNullOrEmpty(i.Logo));

            if(Info is null)
            {
                _logger.LogInformation("Information about given symbol not found");
                throw new Exception("Unexpected exception");
            }

            Price = priceResponse;
        }
    }
}
