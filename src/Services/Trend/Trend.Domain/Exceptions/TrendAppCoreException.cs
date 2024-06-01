namespace Trend.Domain.Exceptions
{
    public class TrendAppCoreException : Exception
    {
        public string UserMessage { get; set; }

        public TrendAppCoreException() : base("Unknown exception")
        {
            UserMessage = "Unknown exception";
        }

        public TrendAppCoreException(string message) : base(message)
        {
            UserMessage = message ?? "Unknown exception";
        }

        public TrendAppCoreException(string developerMessage, string userMessage) : base(developerMessage)
        {
            UserMessage = userMessage ?? "Unknown exception";
        }
    }
}
