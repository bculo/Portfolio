using Crypto.Core.Exceptions;
using FluentValidation;
using MediatR;

namespace Crypto.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any())
                return await next();
            
            var context = new ValidationContext<TRequest>(request);
            var validationTasks = validators.Select(v => v.ValidateAsync(context, cancellationToken));
            var validationResults = await Task.WhenAll(validationTasks);

            var failures = validationResults
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count == 0) 
                return await next();
            
            var errors = failures
                .GroupBy(e => e.PropertyName)
                .ToDictionary(x => x.Key, y => y.Select(i => i.ErrorMessage).ToArray());

            throw new CryptoCoreValidationException(errors);
        }
    }
}
