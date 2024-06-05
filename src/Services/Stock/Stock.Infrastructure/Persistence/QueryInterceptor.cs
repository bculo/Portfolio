using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Stock.Infrastructure.Persistence;

public class QueryInterceptor(ILogger<QueryInterceptor> logger) : DbCommandInterceptor
{
    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        logger.LogTrace("Generated command: {Command}", command.CommandText);
        
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        logger.LogTrace("Generated command: {Command}", command.CommandText);
        
        return base.ReaderExecuting(command, eventData, result);
    }
}