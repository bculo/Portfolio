using AutoMapper;
using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
using Crypto.Application.Models.Price;
using Crypto.Core.Entities;
using Crypto.Core.Interfaces;
using MediatR;
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

        public CryptoInfoDataDto? Info { get; set; }

        public AddNewCryptoCommandHandler(IUnitOfWork work, 
            IMapper mapper, 
            ICryptoInfoService infoService,
            ICryptoPriceService priceService)
        {
            _work = work;
            _mapper = mapper;
            _infoService = infoService;
            _priceService = priceService;
        }

        public async Task<Unit> Handle(AddNewCryptoCommand request, CancellationToken cancellationToken)
        {
            var item = await _work.CryptoRepository.FindSingle(i => i.Symbol.ToLower() == request.Symbol.ToLower());

            if(item != null)
            {
                throw new Exception("Item with given symbol already exists");
            }

            var cryptoInfoTask = _infoService.FetchData(request.Symbol);
            var cryptoPriceTask = _priceService.GetPriceInfo(request.Symbol);

            await Task.WhenAll(cryptoInfoTask, cryptoPriceTask);

            var info = cryptoInfoTask.Result;
            var price = cryptoPriceTask.Result;

            if(info is null || price is null)
            {
                throw new Exception("Provided symbol not supported");
            }

            ExtractDataInfo(info, request.Symbol);

            var newInstance = CreateNewInstance(price);

            await _work.CryptoRepository.Add(newInstance);
            await _work.Commit();

            return Unit.Value;
        }

        private Core.Entities.Crypto CreateNewInstance(CryptoPriceSingleResponseDto price)
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
                Price = price.Prices["EUR"]
            });
            
            return newCrypto;
        }

        private void ExtractDataInfo(CryptoInfoResponseDto infoResponse, string symbol)
        {
            var cryptoData = infoResponse.Data.Values.FirstOrDefault();

            if (cryptoData is null || !cryptoData.Any())
            {
                throw new Exception("Invalid crypto data");
            }

            Info = cryptoData.FirstOrDefault(i => i.Symbol.ToLower() == symbol.ToLower()
                                    && !string.IsNullOrEmpty(i.Description)
                                    && !string.IsNullOrEmpty(i.Name)
                                    && !string.IsNullOrEmpty(i.Logo));

            if(Info is null)
            {
                throw new Exception("Provided symbol not supported");
            }
        }
    }
}
