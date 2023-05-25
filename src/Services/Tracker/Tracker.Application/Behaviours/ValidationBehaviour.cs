using FluentValidation;
using MediatR;
using Tracker.Core.Exceptions;

namespace Tracker.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationTasks = _validators.Select(v => v.ValidateAsync(context, cancellationToken));

        var validationResults = await Task.WhenAll(validationTasks);

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count == 0)
        {
            return await next();
        }
        
        var errors = failures
            .GroupBy(e => e.PropertyName)
            .ToDictionary(x => x.Key, y => y.Select(i => i.ErrorMessage).ToArray());

        
        //TODO add new exception type
        throw new TrackerCoreException("VALIDATION ERRORS");
    }
}