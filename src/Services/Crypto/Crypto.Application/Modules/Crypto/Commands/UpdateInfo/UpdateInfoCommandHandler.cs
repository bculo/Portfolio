using Crypto.Application.Interfaces.Information;
using Crypto.Application.Interfaces.Information.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Exceptions;
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
        
        public UpdateInfoCommandHandler(IUnitOfWork work, 
            ICryptoInfoService infoService,
            IPublishEndpoint publish)
        {
            _work = work;
            _infoService = infoService;
            _publish = publish;
        }

        public async Task Handle(UpdateInfoCommand request, CancellationToken ct)
        {
            var entity = await _work.CryptoRepo.First(
                i => i.Symbol.ToLower() == request.Symbol.ToLower(), 
                track: true,
                ct: ct);
            CryptoCoreNotFoundException.ThrowIfNull(entity, $"Item with symbol {request.Symbol} not found");
            
            var infoResponse = await _infoService.GetInformation(request.Symbol, ct);
            
            await UpdateInstance(entity, infoResponse, ct);
            
            await _publish.Publish(new CryptoInfoUpdated
            {
                Description = entity.Description,
                Id = entity.Id,
                Name = entity.Name,
                Symbol = entity.Symbol,
                Website = entity.WebSite
            }, ct);
        }

        private async Task UpdateInstance(Core.Entities.Crypto entity, 
            CryptoInformation infoResponse, 
            CancellationToken ct)
        {
            entity.Description = infoResponse.Description;
            entity.WebSite = infoResponse.Website;
            entity.Name = infoResponse.Name;
            entity.Logo = infoResponse.Logo;
            entity.SourceCode = infoResponse.SourceCode;

            await _work.Commit(ct);
        }
    }
}
