using AutoMapper;
using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Commands.UpdateInfo
{
    public class UpdateInfoCommandHandler : IRequestHandler<UpdateInfoCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly ICryptoInfoService _infoService;

        public CryptoInfoDataDto? Info { get; set; }

        public UpdateInfoCommandHandler(IUnitOfWork work, ICryptoInfoService infoService)
        {
            _work = work;
            _infoService = infoService;
        }

        public async Task<Unit> Handle(UpdateInfoCommand request, CancellationToken cancellationToken)
        {
            var entity = await _work.CryptoRepository.FindSingle(i => i.Symbol.ToLower() == request.Symbol.ToLower());

            if (entity is null)
            {
                throw new CryptoCoreException("Item with symbol {0} not found", request.Symbol);
            }

            var infoResponse = await _infoService.FetchData(request.Symbol);

            if(infoResponse is null)
            {
                throw new CryptoCoreException("Provided symbol not supported");
            }

            ParseData(infoResponse, request.Symbol);

            await UpdateInstance(entity);

            return Unit.Value;
        }

        private async Task UpdateInstance(Core.Entities.Crypto entity)
        {
            entity.Name = Info!.Name;
            entity.Description = Info!.Description;
            entity.Logo = Info!.Logo;
            entity.WebSite = Info!.Urls["website"]?.FirstOrDefault() ?? null;
            entity.SourceCode = Info!.Urls["source_code"]?.FirstOrDefault() ?? null;
        }

        private void ParseData(CryptoInfoResponseDto infoResponse, string symbol)
        {
            var cryptoData = infoResponse.Data.Values.FirstOrDefault();

            if (cryptoData is null || !cryptoData.Any())
            {
                throw new Exception("Unexpected exception");
            }

            Info = cryptoData.FirstOrDefault(i => i.Symbol.ToLower() == symbol.ToLower()
                                    && !string.IsNullOrEmpty(i.Description)
                                    && !string.IsNullOrEmpty(i.Name)
                                    && !string.IsNullOrEmpty(i.Logo));

            if (Info is null)
            {
                throw new Exception("Unexpected exception");
            }
        }
    }
}
