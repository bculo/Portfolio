namespace Stock.Core.Exceptions.Codes;

public record ErrorCode(string Code, string Message);

public record NotFoundErrorCode(string Code, string Message) : ErrorCode(Code, Message);