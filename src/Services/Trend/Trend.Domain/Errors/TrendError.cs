namespace Trend.Domain.Errors;

public record TrendError(string Code, string Description = null);