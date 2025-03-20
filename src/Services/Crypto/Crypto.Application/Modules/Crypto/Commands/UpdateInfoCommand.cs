using Crypto.Application.Common.Extensions;
using Crypto.Application.Interfaces.Information;
using Crypto.Application.Interfaces.Information.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Exceptions;
using Events.Common.Crypto;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands;

public record UpdateInfoCommand(string Symbol) : IRequest;

public class UpdateInfoCommandValidator : AbstractValidator<UpdateInfoCommand>
{
    public UpdateInfoCommandValidator()
    {
        RuleFor(i => i.Symbol).WithSymbolRule();
    }
}

public class UpdateInfoCommandHandler(
    IUnitOfWork work,
    ICryptoInfoService infoService,
    IPublishEndpoint publish)
    : IRequestHandler<UpdateInfoCommand>
{
    public async Task Handle(UpdateInfoCommand request, CancellationToken ct)
    {
        var entity = await work.CryptoRepo.First(
            i => i.Symbol.ToLower() == request.Symbol.ToLower(), 
            track: true,
            ct: ct);

        if (entity == null)
        {
            throw new CryptoCoreNotFoundException($"Item with symbol {request.Symbol} not found");
        }
            
        var infoResponse = await infoService.GetInformation(request.Symbol, ct);
            
        await UpdateInstance(entity, infoResponse, ct);
            
        await publish.Publish(new InfoUpdated
        {
            Description = entity.Description,
            Id = entity.Id,
            Name = entity.Name,
            Symbol = entity.Symbol,
            Website = entity.WebSite
        }, ct);
    }

    private async Task UpdateInstance(Core.Entities.CryptoEntity entity, 
        CryptoInfoDetailsResponse infoResponse, 
        CancellationToken ct)
    {
        entity.Description = infoResponse.Description;
        entity.WebSite = infoResponse.Website;
        entity.Name = infoResponse.Name;
        entity.Logo = infoResponse.Logo;
        entity.SourceCode = infoResponse.SourceCode;

        await work.Commit(ct);
    }
}