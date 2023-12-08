namespace User.Application.Common.Exceptions;

public class PortfolioUserKeyCloakIntegrationError : PortfolioUserCoreException
{
    public PortfolioUserKeyCloakIntegrationError(string systemMessage, string userMessage)
        : base(systemMessage, userMessage)
    {

    }
}