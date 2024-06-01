using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using SharpCompress.Common;
using Trend.Application.Services.Models;

namespace Trend.Application.Services;

public abstract class ServiceBase(IServiceProvider provider)
{
    public IServiceProvider Provider { get; init; } = provider;

    private IValidator<T> GetValidator<T>() where T : class
    {
        if (Provider.GetRequiredService(typeof(IValidator<T>)) is not IValidator<T> validator)
        {
            throw new ArchiveException($"Can't fetch Validator instance of type {typeof(T).Name} from IServiceProvider");
        }

        return validator;
    }

    protected async Task<ValidationRes> Validate<T>(T instanceToValidate, CancellationToken ct = default) 
        where T : class
    {
        var validationResult = await GetValidator<T>().ValidateAsync(instanceToValidate, ct);
        
        return new ValidationRes
        {
            Errors = validationResult.ToDictionary(),
            IsValid = validationResult.IsValid
        };
    }
}