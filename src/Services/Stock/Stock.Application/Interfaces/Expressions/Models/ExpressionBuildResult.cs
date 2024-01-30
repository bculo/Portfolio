using System.Linq.Expressions;

namespace Stock.Application.Interfaces.Expressions.Models;

public record ExpressionBuildResult<T>(int ExpressionCount, Expression<Func<T, bool>>[] Expressions);