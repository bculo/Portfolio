using Crypto.Application.Interfaces.Information;
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
            IPublishEndpoint publish,
            IDateTimeProvider time)
        {
            _work = work;
            _infoService = infoService;
            _publish = publish;
        }

        public async Task Handle(UpdateInfoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // var entity = await _work.CryptoRepository.FindSingle(i => i.Symbol!.ToLower() == request.Symbol!.ToLower());
            // CryptoCoreException.ThrowIfNull(entity, $"Item with symbol {request.Symbol} not found");
            //
            // var infoResponse = await _infoService.GetInformation(request.Symbol);
            // CryptoCoreException.ThrowIfNull(entity, "Provided symbol not supported");
            //
            // ParseData(infoResponse, request.Symbol);
            //
            // await UpdateInstanceValues(entity);
            //
            // await _publish.Publish(new CryptoInfoUpdated
            // {
            //     Description = entity.Description,
            //     Id = entity.Id,
            //     Name = entity.Name,
            //     Symbol = entity.Symbol,
            //     Website = entity.WebSite
            // });
        }
    }
}
