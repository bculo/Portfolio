using Stock.Core.Models.Common;

namespace Stock.Core.Errors;

public record ErrorCode(string Code, string Message);

public record NotFoundErrorCode(string Code, string Message) : ErrorCode(Code, Message);