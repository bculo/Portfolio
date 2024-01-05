using Dtos.Common;
using Trend.Application.Interfaces;
using Trend.Domain.Enums;

namespace Trend.Application.Services;

public class DictionaryService : IDictionaryService
{
    public Task<List<KeyValueElementDto>> GetContextTypes(CancellationToken token)
    {
        var instances = ContextType
            .GetContextTypes()
            .Select(item => new KeyValueElementDto
            {
                Key = item,
                Value = item.ToString()
            }).ToList();
            
        return Task.FromResult(instances);
    }

    public Task<List<KeyValueElementDto>> GetSearchEngines(CancellationToken token)
    {
        var instances = SearchEngine
            .GetSearchEngines()
            .Select(item => new KeyValueElementDto
            {
                Key = item,
                Value = item.ToString()
            }).ToList();
            
        return Task.FromResult(instances);
    }

    public Task<List<KeyValueElementDto>> GetActiveFilterOptions(CancellationToken token)
    {
        var instances = ActiveFilter
            .GetPossibleFilters()
            .Select(item => new KeyValueElementDto
            {
                Key = item,
                Value = item.ToString()
            }).ToList();
            
        return Task.FromResult(instances);
    }
}