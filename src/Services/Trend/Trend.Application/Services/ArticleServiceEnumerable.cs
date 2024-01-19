using System.Runtime.CompilerServices;
using AutoMapper;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Enums;

namespace Trend.Application.Services;

public class ArticleServiceEnumerable : IArticleServiceEnumerable
{
    private readonly IArticleRepository _articleRepo;
    private readonly IMapper _mapper;

    public ArticleServiceEnumerable(IArticleRepository articleRepo, IMapper mapper)
    {
        _articleRepo = articleRepo;
        _mapper = mapper;
    }
    
    public async IAsyncEnumerable<ArticleResDto> GetAllEnumerable([EnumeratorCancellation] CancellationToken token = default)
    {
        await foreach(var entity in _articleRepo.GetAllEnumerable(token))
        {
            yield return _mapper.Map<ArticleResDto>(entity);
        }
    }

    public async IAsyncEnumerable<ArticleResDto> GetLatestNewsEnumerable([EnumeratorCancellation] CancellationToken token = default)
    {
        await foreach(var entity in _articleRepo.GetActiveItemsEnumerable(token))
        {
            yield return _mapper.Map<ArticleResDto>(entity);
        }
    }

    public async IAsyncEnumerable<ArticleResDto> GetLatestNewsEnumerable(ContextType type, 
        [EnumeratorCancellation] CancellationToken token = default)
    {
        await foreach (var entity in _articleRepo.GetActiveArticlesEnumerable(type, token))
        {
            yield return _mapper.Map<ArticleResDto>(entity);
        }
    }
}