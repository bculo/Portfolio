namespace Trend.Grpc.Interceptors.Models
{
    public class ValidationResponse
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
