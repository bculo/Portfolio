namespace Stock.Application.Common.Models;

public record PageResultDto<T>(long TotalCount, IEnumerable<T> Items);