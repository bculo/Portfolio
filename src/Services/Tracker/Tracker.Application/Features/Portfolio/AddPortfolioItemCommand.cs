using FluentValidation;
using MediatR;
using Tracker.Application.Interfaces.Integration;
using Tracker.Core.Enums;

namespace Tracker.Application.Features.Portfolio;

public record AddPortfolioItemCommand(string Symbol, FinancialAssetType AssetType) : IRequest;

public class AddPortfolioItemCommandValidator : AbstractValidator<AddPortfolioItemCommand>
{
    public AddPortfolioItemCommandValidator()
    {
        RuleFor(i => i.Symbol).NotEmpty();
    }
}

public class AddPortfolioItemHandler(IFinancialAssetAdapterFactory adapterFactory) 
    : IRequestHandler<AddPortfolioItemCommand>
{
    public async Task Handle(AddPortfolioItemCommand request, CancellationToken ct)
    {
        var adapter = adapterFactory.GetAdapter(request.AssetType);
        var result = await adapter.FetchAsset(request.Symbol, ct);
    }
}