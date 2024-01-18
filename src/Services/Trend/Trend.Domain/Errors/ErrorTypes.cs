namespace Trend.Domain.Errors;

public record CoreError(string Code, string Description);

public record NotFoundError(string Code, string Description) : CoreError(Code, Description);