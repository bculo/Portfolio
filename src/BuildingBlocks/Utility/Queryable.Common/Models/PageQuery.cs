namespace Queryable.Common.Models;

public record PageQuery
{
    public int Page { get; set; }
    public int Take { get; set; }
}