namespace Trend.Application.Services.Models;

public class ValidationRes
{
    public bool IsValid { get; set; }
    public IDictionary<string, string[]> Errors { get; set; }
}