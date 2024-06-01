namespace Trend.Application.Interfaces
{
    public interface ILanguageService<T> where T : class
    {
        string Get(string identifier);
        string GetCurrentCulture();
    }
}
