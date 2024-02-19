using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Time.Abstract.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.UpdateInfo
{
    public class UpdateInfoCommandHandler : IRequestHandler<UpdateInfoCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly ICryptoInfoService _infoService;
        private readonly IPublishEndpoint _publish;

        public CryptoInfoDataDto? Info { get; set; }

        public UpdateInfoCommandHandler(IUnitOfWork work, 
            ICryptoInfoService infoService,
            IPublishEndpoint publish,
            IDateTimeProvider time)
        {
            _work = work;
            _infoService = infoService;
            _publish = publish;
        }

        public async Task Handle(UpdateInfoCommand request, CancellationToken cancellationToken)
        {
            var entity = await _work.CryptoRepository.FindSingle(i => i.Symbol!.ToLower() == request.Symbol!.ToLower());
            CryptoCoreException.ThrowIfNull(entity, $"Item with symbol {request.Symbol} not found");

            var infoResponse = await _infoService.FetchData(request.Symbol);
            CryptoCoreException.ThrowIfNull(entity, "Provided symbol not supported");

            ParseData(infoResponse, request.Symbol);

            await UpdateInstanceValues(entity);

            await _publish.Publish(new CryptoInfoUpdated
            {
                Description = entity.Description,
                Id = entity.Id,
                Name = entity.Name,
                Symbol = entity.Symbol,
                Website = entity.WebSite
            });
        }

        private async Task UpdateInstanceValues(Core.Entities.Crypto entity)
        {
            entity.Name = Info!.Name;
            entity.Description = Info!.Description;
            entity.Logo = Info!.Logo;
            entity.WebSite = Info!.Urls["website"]?.FirstOrDefault() ?? null;
            entity.SourceCode = Info!.Urls["source_code"]?.FirstOrDefault() ?? null;

            await _work.Commit();
        }

        private void ParseData(CryptoInfoResponseDto infoResponse, string symbol)
        {
            var cryptoData = infoResponse.Data.Values.FirstOrDefault();

            CryptoCoreException.ThrowIfEmpty(cryptoData, "Unexpected exception");

            Info = cryptoData.FirstOrDefault(i => i.Symbol.ToLower() == symbol.ToLower()
                                    && !string.IsNullOrEmpty(i.Description)
                                    && !string.IsNullOrEmpty(i.Name)
                                    && !string.IsNullOrEmpty(i.Logo));

            CryptoCoreException.ThrowIfNull(Info, "Unexpected exception");
        }
    }
}
