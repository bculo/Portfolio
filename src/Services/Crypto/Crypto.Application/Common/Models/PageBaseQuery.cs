namespace Crypto.Application.Common.Models;

public record PageBaseQuery
{
    public int Page { get; set; }
    public int Take { get; set; }
}



