using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Exceptions;

namespace Trend.Application.Utils.Validation
{
    public static class ValidationUtils
    {
        public static void Validate<T>(T instance, AbstractValidator<T> validator) where T : class
        {
            var result = validator.Validate(instance);

            if(result.IsValid)
            {
                return;
            }

            var errors = result.Errors.GroupBy(i => i.PropertyName)
                .ToDictionary(i => i.Key, y => y.Select(i => i.ErrorMessage).ToList());

            throw new TrendValidationException(errors);
        }
    }
}
