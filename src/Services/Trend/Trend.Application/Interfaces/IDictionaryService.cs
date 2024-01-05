using Dtos.Common;

namespace Trend.Application.Interfaces;

public interface IDictionaryService
{
    Task<List<KeyValueElementDto>> GetContextTypes(CancellationToken token);
    Task<List<KeyValueElementDto>> GetSearchEngines(CancellationToken token);
    
    Task<List<KeyValueElementDto>> GetActiveFilterOptions(CancellationToken token);
}