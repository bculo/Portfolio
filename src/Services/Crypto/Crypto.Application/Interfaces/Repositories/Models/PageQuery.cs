namespace Crypto.Application.Interfaces.Repositories.Models;

public record PageQuery(int Page, int Take)
{
    public int Skip => (Page - 1) * Take;
    
    public static implicit operator (int, int)(PageQuery context) => (context.Skip, context.Take);
}