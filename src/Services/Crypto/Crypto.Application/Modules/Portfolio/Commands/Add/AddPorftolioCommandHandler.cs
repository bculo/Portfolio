using Crypto.Application.Interfaces.Services;
using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Portfolio.Commands.Add
{
    public class AddPorftolioCommandHandler : IRequestHandler<AddPorftolioCommand, string>
    {
        private readonly ILogger<AddPorftolioCommandHandler> _logger;
        private readonly IUnitOfWork _work;
        private readonly IIdentiferHasher _hasher;

        public AddPorftolioCommandHandler(ILogger<AddPorftolioCommandHandler> logger, 
            IUnitOfWork work, 
            IIdentiferHasher hasher)
        {
            _logger = logger;
            _work = work;
            _hasher = hasher;
        }

        public async Task<string> Handle(AddPorftolioCommand request, CancellationToken cancellationToken)
        {
            //TODO add user ID
            string userId = string.Empty;

            var existingPortfolio = await _work.PortfolioRepositry.GetPortfolio(request.Name, userId);

            if(existingPortfolio != null)
            {
                _logger.LogInformation("Portfolio with name not found {0}", request.Name);
                throw new CryptoCoreException($"Portfolio with given name {request.Name} already exist");
            }

            var newPortfolio = new Core.Entities.Portfolio(request.Name);

            await _work.PortfolioRepositry.Add(newPortfolio);
            await _work.Commit();

            _logger.LogTrace("Portfolio with name {0} created", request.Name);

            var hashedId = _hasher.Encode(newPortfolio.Id);

            return hashedId;
        }
    }
}
