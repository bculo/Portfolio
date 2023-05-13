using MediatR;
using Microsoft.Extensions.Logging;

namespace Mail.Application.Behaviours;

public class ExceptionBehaviour<TRequest, TResponse>  : IPipelineBehavior<TRequest, TResponse>
{
    protected readonly ILogger<TRequest> _logger;

    public ExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}