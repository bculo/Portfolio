using System.Linq.Expressions;
using System.Reflection;
using Queryable.Common.Utilities;

namespace Queryable.Common.Extensions;

public enum Operation
{
    Equals,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual,
    Contains,
    StartsWith,
    EndsWith
}

public class QueryFilter
{
    public string PropertyName { get; init; } = default!;
    public Operation Operation { get; init; }
    public object Value { get; init; } = default!;
}

public static class ExpressionBuilder
{
    private static readonly MethodInfo ContainsMethod 
        = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
    
    private static readonly MethodInfo StartsWithMethod 
        = typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!;
    
    private static readonly MethodInfo EndsWithMethod 
        = typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!;


    public static IQueryable<T> Where<T>(this IQueryable<T> query, List<QueryFilter> filters)
    {
        var target = Expression.Parameter(typeof(T));
        return query.Provider.CreateQuery<T>(CreateWhereClause<T>(target, query.Expression, filters));
    }

    private static Expression CreateWhereClause<T>(ParameterExpression target, 
        Expression expression, 
        List<QueryFilter> filters)
    {
        var predicate = Expression.Lambda(CreateComparison<T>(target, filters), target);
        return Expression.Call(typeof(System.Linq.Queryable), 
            nameof(System.Linq.Queryable.Where), 
            new[] { target.Type }, 
            expression, 
            Expression.Quote(predicate));
    }

    private static Expression CreateComparison<T>(ParameterExpression target, List<QueryFilter> filters)
    {
        Expression? exp = null;
        filters.ForEach(filter =>
        {
            var memberAccess = CreateMemberAccess(target, filter.PropertyName);
            var exp2 = GetExpression<T>(memberAccess, filter);
            exp = exp == null ? exp2 : Expression.Or(exp, exp2);
        });
        return exp;
    }

    private static Expression CreateMemberAccess(Expression target, string selector)
    {
        return selector.Split('.').Aggregate(target, Expression.PropertyOrField);
    }
    
    private static Expression GetExpression<T>(Expression member, QueryFilter queryFilter)
    {
        var actualValue = ExpressionUtilities.GetSelector<T>(queryFilter.PropertyName, queryFilter.Value);
        return queryFilter.Operation switch
        {
            Operation.Equals => Expression.Equal(member, actualValue),
            Operation.GreaterThan => Expression.GreaterThan(member, actualValue),
            Operation.GreaterThanOrEqual => Expression.GreaterThanOrEqual(member, actualValue),
            Operation.LessThan => Expression.LessThan(member, actualValue),
            Operation.LessThanOrEqual => Expression.LessThanOrEqual(member, actualValue),
            Operation.Contains => Expression.Call(member, ContainsMethod, actualValue),
            Operation.StartsWith => Expression.Call(member, StartsWithMethod, actualValue),
            Operation.EndsWith => Expression.Call(member, EndsWithMethod, actualValue),
            _ => null
        };
    }
}
