using FluentValidation;
using MediatR;
using Tracker.Core.Enums;

namespace Tracker.Application.Features.Favorite;

public static class AddFavorite
{
    public class Command : IRequest
    {
        public string Symbol { get; set; }
        public FinancalAssetType Type { get; set; }
    }
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(i => i.Symbol)
                .NotEmpty();
        }
    }

    //public class Handler : IRequestHandler<Command>
    //{
    //    private readonly ILogger<Handler> _logger;
    //    private readonly TrackerDbContext _context;
    //    private readonly ITrackerCacheService _cache;
    //    private readonly IAuth0AccessTokenReader _tokenReader;
        
    //    public Handler(ILogger<Handler> logger, 
    //        TrackerDbContext context, 
    //        ITrackerCacheService cache, 
    //        IAuth0AccessTokenReader tokenReader)
    //    {
    //        _logger = logger;
    //        _context = context;
    //        _cache = cache;
    //        _tokenReader = tokenReader;
    //    }

    //    public async Task Handle(Command request, CancellationToken cancellationToken)
    //    {
    //        var cacheKey = CacheKeyUtilities.CombineKey(request.Type, request.Symbol);
    //        var cacheItem = await _cache.Get<FinancialAssetItem>(cacheKey);
    //        if (cacheItem is null)
    //        {
    //            throw new TrackerCoreException($"Financial asset with given symbol {request.Symbol} not available.");
    //        }
            
    //        var dbInstance = await _context.Favorites
    //            .Where(i => i.Symbol.ToLower() == request.Symbol.ToLower()
    //                && i.AssetType == request.Type)
    //            .AsNoTracking()
    //            .FirstOrDefaultAsync(cancellationToken);

    //        if (dbInstance is not null)
    //        {
    //            throw new TrackerCoreException("Financial asset already added to favorites.");
    //        }

    //        var newFavoriteAsset = new FavoriteAsset
    //        {
    //            Symbol = request.Symbol,
    //            AssetType = request.Type,
    //            UserId = _tokenReader.GetIdentifier(),
    //        };

    //        _context.Favorites.Add(newFavoriteAsset);
    //        await _context.SaveChangesAsync(cancellationToken);
    //    }
    //}
}