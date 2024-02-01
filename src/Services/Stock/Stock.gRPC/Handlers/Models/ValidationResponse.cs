namespace Stock.gRPC.Handlers.Models;

public class ValidationResponse
{
    public Dictionary<string, string[]> Errors { get; set; } = default!;
}