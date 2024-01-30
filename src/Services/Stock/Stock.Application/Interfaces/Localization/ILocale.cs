using Stock.Core.Exceptions.Codes;

namespace Stock.Application.Interfaces.Localization;

public interface ILocale
{
    string Get(string code);
    string Get(ErrorCode code);
}