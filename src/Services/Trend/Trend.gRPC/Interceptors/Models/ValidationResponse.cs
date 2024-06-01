namespace Trend.gRPC.Interceptors.Models
{
    public class ValidationResponse
    {
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public Dictionary<string, List<string>> Errors { get; set; } = default!;
    }
}
