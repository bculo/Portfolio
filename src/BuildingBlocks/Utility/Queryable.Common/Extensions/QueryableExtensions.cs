using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Queryable.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyWhereAny<T>(this IQueryable<T> source, 
        params Expression<Func<T, bool>>[] predicates)
    {
        var parameter = Expression.Parameter(typeof(T));

        return source.Where(
            Expression.Lambda<Func<T, bool>>(
                predicates.Aggregate<Expression<Func<T, bool>>, Expression>(
                    Expression.Constant(false),
                    (current, predicate) => Expression.OrElse(
                        current,
                        new ParameterReplaceVisitor(predicate.Parameters[0], parameter).Visit(predicate.Body))),
                parameter));
    }

    public static IQueryable<T> ApplyWhereAll<T>(this IQueryable<T> source, 
        params Expression<Func<T, bool>>[] predicates)
    {
        foreach (var predicate in predicates)
        {
            source = source.Where(predicate);
        }

        return source;
    }

    public static IQueryable<T> ApplyInclude<T>(this IQueryable<T> source,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = default)
    {
        if (includes is null)
        {
            return source;
        }

        return includes(source);
    }
    
    public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> source,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default)
    {
        if (orderBy is null)
        {
            return source;
        }

        return orderBy(source);
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> source, int skip, int take)
    {
        return source.Skip(skip).Take(take);
    }

    public static IQueryable<T> ApplyTracking<T>(this IQueryable<T> source, bool tracking) where T : class
    {
        return tracking ? source.AsTracking() : source.AsNoTracking();
    }
    
    public static IQueryable<T> ApplySplitQuery<T>(this IQueryable<T> source, bool splitQuery) where T : class
    {
        return splitQuery ? source.AsSplitQuery() : source;
    }
}

public class ParameterReplaceVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _from;
    private readonly ParameterExpression _to;
   
    public ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to)
    {
        _from = from;
        _to = to;
    }
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == _from ? _to : base.VisitParameter(node);
    }
}