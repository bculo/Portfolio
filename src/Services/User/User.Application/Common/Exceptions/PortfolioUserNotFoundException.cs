namespace User.Application.Common.Exceptions
{
    public class PortfolioUserNotFoundException : PortfolioUserCoreException
    {
        public PortfolioUserNotFoundException(string userMessage)
            : base("Item not found", userMessage)
        {
            
        }
    }
}
