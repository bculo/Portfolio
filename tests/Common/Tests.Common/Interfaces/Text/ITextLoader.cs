namespace Tests.Common.Interfaces.Text;

public interface ITextLoader
{
    Task<string> LoadAsync();
}