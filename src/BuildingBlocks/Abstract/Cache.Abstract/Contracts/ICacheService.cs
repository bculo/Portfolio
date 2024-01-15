namespace Cache.Abstract.Contracts
{
    public interface ICacheService
    {
        Task<T?> Get<T>(string identifier) where T : class;
        Task<string?> Get(string identifier);
        Task Add(string identifier, object instance);
        Task AddWithSlidingExp(string identifier, object instance, TimeSpan span);
        Task AddWithAbsoluteExp(string identifier, object instance, TimeSpan span);
    }
}
