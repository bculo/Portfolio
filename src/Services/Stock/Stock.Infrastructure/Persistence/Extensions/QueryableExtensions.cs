using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Stock.Core.Models.Common;

namespace Stock.Infrastructure.Persistence.Extensions;

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
        Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
    {
        if (includes is null)
        {
            return source;
        }

        return includes(source);
    }
    
    public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> source,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
    {
        if (orderBy is null)
        {
            return source;
        }

        return orderBy(source);
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> source,
        PageQuery page)
    {
        return source.Skip(page.Skip).Take(page.Take);
    }

    public static IQueryable<T> ApplyTracking<T>(this IQueryable<T> source, bool tracking) where T : class
    {
        return tracking ? source.AsTracking() : source.AsNoTracking();
    }
}