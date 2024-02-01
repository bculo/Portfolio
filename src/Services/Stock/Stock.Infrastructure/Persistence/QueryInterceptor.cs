using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Stock.Infrastructure.Persistence;

public class QueryInterceptor : DbCommandInterceptor
{
    private readonly ILogger<QueryInterceptor> _logger;

    public QueryInterceptor(ILogger<QueryInterceptor> logger)
    {
        _logger = logger;
    }
    
    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogTrace("Generated command: {Command}", command.CommandText);
        
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        _logger.LogTrace("Generated command: {Command}", command.CommandText);
        
        return base.ReaderExecuting(command, eventData, result);
    }
}