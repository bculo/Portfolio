namespace Trend.API.Interfaces
{
    public interface IGoogleSearchService
    {
        Task<string> Search(string searchDefinition);
    }
}
