namespace Crypto.Application.Interfaces.Repositories.Models;

public record PageRepoQuery(int Page, int Take)
{
    public int Skip => (Page - 1) * Take;
    
    public static implicit operator (int, int)(PageRepoQuery context) => (context.Skip, context.Take);
}