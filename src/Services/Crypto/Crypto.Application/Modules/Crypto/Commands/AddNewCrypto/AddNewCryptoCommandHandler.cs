using AutoMapper;
using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
using Crypto.Application.Models.Price;
using Crypto.Core.Entities;
using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewCrpyto
{
    public class AddNewCryptoCommandHandler : IRequestHandler<AddNewCryptoCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;
        private readonly ICryptoInfoService _infoService;
        private readonly ICryptoPriceService _priceService;
        private readonly ILogger<AddNewCryptoCommandHandler> _logger;

        public CryptoInfoDataDto? Info { get; set; }
        public decimal Price { get; set; }

        public AddNewCryptoCommandHandler(IUnitOfWork work, 
            IMapper mapper, 
            ICryptoInfoService infoService,
            ICryptoPriceService priceService,
            ILogger<AddNewCryptoCommandHandler> logger)
        {
            _work = work;
            _mapper = mapper;
            _infoService = infoService;
            _priceService = priceService;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddNewCryptoCommand request, CancellationToken cancellationToken)
        {
            var item = await _work.CryptoRepository.FindSingle(i => i.Symbol.ToLower() == request.Symbol.ToLower());

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

            await _work.CryptoRepository.Add(newInstance);
            await _work.Commit();

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
                WebSite = Info!.Urls["website"]?.FirstOrDefault() ?? null,
                SourceCode = Info!.Urls["source_code"]?.FirstOrDefault() ?? null
            };

            newCrypto.Explorers = Info!.Urls["explorer"]?.Select(i => new CryptoExplorer
            {
                Url = i
            }).ToList() ?? new List<CryptoExplorer>();

            newCrypto.Prices.Add(new CryptoPrice
            {
                Price = Price
            });
            
            return newCrypto;
        }

        private void ParseData(CryptoInfoResponseDto infoResponse, CryptoPriceSingleResponseDto priceResponse, string symbol)
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

            if(priceResponse.Prices == null || !priceResponse.Prices.Keys.All(i => i != "USD"))
            {
                _logger.LogInformation("Price for given symbol not found");
                throw new Exception("Unexpected exception");
            }

            Price = priceResponse.Prices["USD"];
        }
    }
}
