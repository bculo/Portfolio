using System.Diagnostics;
using Dtos.Common;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
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
            .GetPossibleOptions()
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
            .GetPossibleOptions()
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
            .GetPossibleOptions()
            .Select(item => new KeyValueElementDto
            {
                Key = item,
                Value = item.ToString()
            }).ToList();
            
        return Task.FromResult(instances);
    }

    public Task<List<KeyValueElementDto>> GetSortFilterOptions(CancellationToken token)
    {
        var instances = SortType
            .GetPossibleOptions()
            .Select(item => new KeyValueElementDto
            {
                Key = item,
                Value = item.ToString()
            }).ToList();
            
        return Task.FromResult(instances);
    }
}