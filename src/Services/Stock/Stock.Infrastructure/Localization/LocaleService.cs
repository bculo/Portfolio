using Microsoft.Extensions.Localization;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Resources;
using Stock.Core.Exceptions.Codes;

namespace Stock.Infrastructure.Localization;

public class LocaleService(IStringLocalizerFactory factory) : ILocale
{
    private const string DefaultMessage = "Error occurred";

    private IStringLocalizer? GetLocalizer(string code)
    {
        var codePrefix = code.Split(".").FirstOrDefault();
        if (codePrefix is null)
        {
            return null;
        }

        return codePrefix switch
        {
            "Validation" => factory.Create(typeof(ValidationShared)),
            _ => null
        };
    } 
    
    public string Get(string code)
    {
        return GetLocalizer(code)?.GetString(code) ?? DefaultMessage;
    }

    public string Get(ErrorCode error)
    {
        return GetLocalizer(error.Code)?.GetString(error.Code) ?? DefaultMessage;
    }
}