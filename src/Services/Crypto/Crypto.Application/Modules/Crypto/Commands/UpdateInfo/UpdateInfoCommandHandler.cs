using Crypto.Application.Interfaces.Persistence;
using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
using Crypto.Core.Exceptions;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Time.Common.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.UpdateInfo
{
    public class UpdateInfoCommandHandler : IRequestHandler<UpdateInfoCommand>
    {
        private readonly ICryptoDbContext _work;
        private readonly ICryptoInfoService _infoService;
        private readonly IPublishEndpoint _publish;
        private readonly IDateTime _time;

        public CryptoInfoDataDto? Info { get; set; }

        public UpdateInfoCommandHandler(ICryptoDbContext work, 
            ICryptoInfoService infoService,
            IPublishEndpoint publish,
            IDateTime time)
        {
            _work = work;
            _infoService = infoService;
            _publish = publish;
            _time = time;
        }

        public async Task<Unit> Handle(UpdateInfoCommand request, CancellationToken cancellationToken)
        {
            var entity = await _work.Cryptos.FirstOrDefaultAsync(i => i.Symbol!.ToLower() == request.Symbol!.ToLower());

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

            await UpdateInstanceValues(entity);

            await _publish.Publish(new CryptoInfoUpdated
            {
                CreatedOn = _time.DateTime,
                Description = entity.Description,
                Id = entity.Id,
                Name = entity.Name,
                Symbol = entity.Symbol,
                Website = entity.WebSite
            });

            return Unit.Value;
        }

        private Task UpdateInstanceValues(Core.Entities.Crypto entity)
        {
            entity.Name = Info!.Name;
            entity.Description = Info!.Description;
            entity.Logo = Info!.Logo;
            entity.WebSite = Info!.Urls["website"]?.FirstOrDefault() ?? null;
            entity.SourceCode = Info!.Urls["source_code"]?.FirstOrDefault() ?? null;

            return Task.CompletedTask;
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
