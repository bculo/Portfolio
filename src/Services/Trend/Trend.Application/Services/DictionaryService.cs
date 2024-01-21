using Dtos.Common;
using Microsoft.Extensions.Logging;
using Trend.Application.Interfaces;
using Trend.Domain.Enums;

namespace Trend.Application.Services;

public class DictionaryService : IDictionaryService
{
    private readonly ILogger<DictionaryService> _logger;

    public DictionaryService(ILogger<DictionaryService> logger)
    {
        _logger = logger;
    }

    public Task<int> GetDefaultAllValue(CancellationToken token)
    {
        return Task.FromResult(999);
    }

    public Task<List<KeyValueElementDto>> GetContextTypes(CancellationToken token)
    {
        var instances = ContextType
            .GetAll()
            .Select(item => new KeyValueElementDto
            {
                Key = item.Value,
                Value = item.DisplayValue
            }).ToList();
            
        return Task.FromResult(instances);
    }

    public Task<List<KeyValueElementDto>> GetSearchEngines(CancellationToken token)
    {
        var instances = SearchEngine
            .GetAll()
            .Select(item => new KeyValueElementDto
            {
                Key = item.Value,
                Value = item.DisplayValue
            }).ToList();
            
        return Task.FromResult(instances);
    }

    public Task<List<KeyValueElementDto>> GetActiveFilterOptions(CancellationToken token)
    {
        var instances = ActiveFilter
            .GetAll()
            .Select(item => new KeyValueElementDto
            {
                Key = item.Value,
                Value = item.DisplayValue
            }).ToList();
            
        return Task.FromResult(instances);
    }

    public Task<List<KeyValueElementDto>> GetSortFilterOptions(CancellationToken token)
    {
        var instances = SortType
            .GetAll()
            .Select(item => new KeyValueElementDto
            {
                Key = item.Value,
                Value = item.DisplayValue
            }).ToList();
            
        return Task.FromResult(instances);
    }
}