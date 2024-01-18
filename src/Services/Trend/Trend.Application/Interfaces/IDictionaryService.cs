using Dtos.Common;

namespace Trend.Application.Interfaces;

public interface IDictionaryService
{
    Task<int> GetDefaultAllValue(CancellationToken token = default);
    Task<List<KeyValueElementDto>> GetContextTypes(CancellationToken token = default);
    Task<List<KeyValueElementDto>> GetSearchEngines(CancellationToken token = default);
    
    Task<List<KeyValueElementDto>> GetActiveFilterOptions(CancellationToken token = default);
    
    Task<List<KeyValueElementDto>> GetSortFilterOptions(CancellationToken token = default);
}