using FluentValidation;
using MediatR;
using Stock.Core.Exceptions;

namespace Stock.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : notnull
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

            if (failures.Count != 0)
            {
                var errors = failures
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(x => x.Key, y => y.Select(i => i.ErrorMessage).ToArray());
                
                throw new StockCoreValidationException(errors);
            }

            return await next();
        }
    }
}
