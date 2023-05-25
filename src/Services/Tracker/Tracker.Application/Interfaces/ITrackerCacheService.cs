namespace Tracker.Application.Interfaces;

public interface ITrackerCacheService
{
    Task<T> Get<T>(string identifier) where T : class;
    Task<string> Get(string identifier);
    Task<List<T>> GetList<T>(string identifier) where T : class;
    Task Add(string identifier, object instance, bool setExpirationTime = true);
}