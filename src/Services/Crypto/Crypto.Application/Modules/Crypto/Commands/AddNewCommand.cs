using Crypto.Application.Common.Extensions;
using Crypto.Application.Interfaces.Information;
using Crypto.Application.Interfaces.Information.Models;
using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Price.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using Crypto.Core.Exceptions;
using Events.Common.Crypto;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Time.Common;

namespace Crypto.Application.Modules.Crypto.Commands;

public record AddNewCommand(string Symbol, Guid? CorrelationId) : IRequest;

public class AddNewCommandValidator : AbstractValidator<AddNewCommand>
{
    public AddNewCommandValidator()
    {
        RuleFor(i => i.Symbol).WithSymbolRule();
    }
}

public class AddNewCommandHandler(
    IUnitOfWork work,
    IPublishEndpoint publish,
    ICryptoPriceService priceService,
    ICryptoInfoService infoService,
    IDateTimeProvider timeProvider)
    : IRequestHandler<AddNewCommand>
{
    public async Task Handle(AddNewCommand request, CancellationToken ct)
    {
        var item = await work.CryptoRepo.First(
            i => EF.Functions.ILike(i.Symbol, request.Symbol),
            ct: ct);

        if(item != null)
        {
            throw new CryptoCoreException($"Item with given symbol {request.Symbol} already exist");
        }

        var cryptoInfoTask = infoService.GetInformation(request.Symbol, ct);
        var cryptoPriceTask = priceService.GetPriceInfo(request.Symbol, ct);

        await Task.WhenAll(cryptoInfoTask, cryptoPriceTask);
        
        var newCrypto = CreateNewCryptoEntity(cryptoInfoTask.Result);
        await work.CryptoRepo.Add(newCrypto, ct);
        await work.Commit(ct);
        
        var newPrice = CreateNewCryptoPriceEntity(cryptoPriceTask.Result, newCrypto.Id);
        await work.CryptoPriceRepo.Add(newPrice, ct);
        await work.Commit(ct);

        await publish.Publish(new NewItemAdded
        {
            Id = newCrypto.Id,
            Name = newCrypto.Name,
            Price = newPrice!.Price,
            Symbol = newCrypto.Symbol,
            CorrelationId = request.CorrelationId ?? Guid.NewGuid()
        }, ct);
        
        await publish.Publish(new EvictRedisListRequest(), ct);
    }

    private CryptoPriceEntity CreateNewCryptoPriceEntity(PriceResponse result, Guid cryptoId)
    {
        return new CryptoPriceEntity
        {
            CryptoEntityId = cryptoId,
            Price = result.Price,
            Time = timeProvider.TimeOffset
        };
    }

    private CryptoEntity CreateNewCryptoEntity(CryptoInformation info)
    {
        return new CryptoEntity
        {
            Id = Guid.NewGuid(),
            Logo = info.Logo,
            Name = info.Name,
            Symbol = info.Symbol,
            Description = info.Description,
            WebSite = info.Website,
            SourceCode = info.SourceCode
        };
    }
}