namespace User.Application.Common.Exceptions
{
    public class PortfolioUserCoreException : Exception
    {
        public string UserMessage { get; set; }

        public PortfolioUserCoreException(string systemMessage, string userMessage)
            : base(systemMessage)
        {
            UserMessage = userMessage;
        }
    }
}
