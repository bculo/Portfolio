using AutoMapper;
using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
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

            var newInstance = _mapper.Map<Core.Entities.Crypto>(request);

            await _work.CryptoRepository.Add(newInstance);
            await _work.Commit();

            return Unit.Value;
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
        }
    }
}
