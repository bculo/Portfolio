using Crypto.Application.Interfaces.Services;
using Crypto.Core.Interfaces;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll
{
    public class UpdatePriceAllCommandHandler : IRequestHandler<UpdatePriceAllCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly ICryptoPriceService _priceService;
        private readonly IPublishEndpoint _publish;
        private readonly IDateTime _time;

        public UpdatePriceAllCommandHandler(IUnitOfWork work,
            ICryptoPriceService priceService,
            IPublishEndpoint publish,
            IDateTime time)
        {
            _work = work;
            _priceService = priceService;
            _publish = publish;
            _time = time;
        }

        public async Task<Unit> Handle(UpdatePriceAllCommand request, CancellationToken cancellationToken)
        {
            var symbols = await _work.CryptoRepository.GetAllSymbols();

            if (!symbols.Any())
            {
                return Unit.Value;
            }

            var response = await _priceService.GetPriceInfo(symbols);

            throw new NotImplementedException();
        }
    }
}
