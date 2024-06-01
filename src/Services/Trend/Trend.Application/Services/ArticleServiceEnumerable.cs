using System.Runtime.CompilerServices;
using AutoMapper;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Enums;

namespace Trend.Application.Services;

public class ArticleServiceEnumerable(IArticleRepository articleRepo, IMapper mapper) : IArticleServiceEnumerable
{
    public async IAsyncEnumerable<ArticleResDto> GetAllEnumerable([EnumeratorCancellation] CancellationToken token = default)
    {
        await foreach(var entity in articleRepo.GetAllEnumerable(token))
        {
            yield return mapper.Map<ArticleResDto>(entity);
        }
    }

    public async IAsyncEnumerable<ArticleResDto> GetLatestEnumerable([EnumeratorCancellation] CancellationToken token = default)
    {
        await foreach(var entity in articleRepo.GetActiveItemsEnumerable(token))
        {
            yield return mapper.Map<ArticleResDto>(entity);
        }
    }

    public async IAsyncEnumerable<ArticleResDto> GetLatestByContextEnumerable(ContextType type, 
        [EnumeratorCancellation] CancellationToken token = default)
    {
        await foreach (var entity in articleRepo.GetActiveEnumerable(type, token))
        {
            yield return mapper.Map<ArticleResDto>(entity);
        }
    }
}