namespace Crypto.API.Filters.Models
{
    public class ValidationErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
