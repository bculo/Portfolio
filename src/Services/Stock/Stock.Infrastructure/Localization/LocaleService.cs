using Microsoft.Extensions.Localization;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Resources;
using Stock.Core.Exceptions.Codes;

namespace Stock.Infrastructure.Localization;

public class LocaleService : ILocale
{
    private const string DEFAULT_MESSAGE = "Error occurred";
    
    private readonly IStringLocalizerFactory _factory;
    
    public LocaleService(IStringLocalizerFactory factory)
    {
        _factory = factory;
    }

    public IStringLocalizer? GetLocalizer(string code)
    {
        var codePrefix = code.Split(".").FirstOrDefault();
        if (codePrefix is null)
        {
            return null;
        }
        
        switch (codePrefix)
        {
            case "Validation":
                return _factory.Create(typeof(ValidationShared));
            default:
                return null;
        }
    } 
    
    public string Get(string code)
    {
        return GetLocalizer(code)?.GetString(code) ?? DEFAULT_MESSAGE;
    }

    public string Get(ErrorCode error)
    {
        return GetLocalizer(error.Code)?.GetString(error.Code) ?? DEFAULT_MESSAGE;
    }
}