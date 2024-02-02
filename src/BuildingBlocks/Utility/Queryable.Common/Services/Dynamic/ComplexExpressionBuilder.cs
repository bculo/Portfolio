using System.Linq.Expressions;
using System.Reflection;
using Queryable.Common.Models;

namespace Queryable.Common.Services.Dynamic;

public class ComplexExpressionBuilder<TModel>
{
    private readonly List<Expression<Func<TModel, bool>>> _expressions = new();
    
    private static readonly MethodInfo ContainsMethod 
        = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
    
    public ComplexExpressionBuilder<TModel> Add<T>(Expression<Func<TModel, T>> propertySelector, EqualFilter<T>? filter)
    {
        if (filter is null || filter.Value is null)
        {
            return this;
        }
        
        var propertySelectorRef = propertySelector.Body;
        var parameter = propertySelector.Parameters[0];
        var constantRef = Expression.Constant(filter.Value);

        var res = Expression.Lambda<Func<TModel, bool>>(
            Expression.Equal(propertySelectorRef, constantRef),
            parameter);

        _expressions.Add(res);
        
        return this;
    }
    
    public ComplexExpressionBuilder<TModel> Add(Expression<Func<TModel, string>> propertySelector, ContainFilter? filter)
    {
        if (filter is null || filter.Value is null)
        {
            return this;
        }
        
        var propertyRef = propertySelector.Body;
        var parameter = propertySelector.Parameters[0];
        var constantRef = Expression.Constant(filter.Value);
        
        var res =Expression.Lambda<Func<TModel, bool>>(
            Expression.Call(propertyRef, ContainsMethod, constantRef),
            parameter);

        _expressions.Add(res);
        
        return this;
    }
    
    public ComplexExpressionBuilder<TModel> Add<T>(Expression<Func<TModel, T>> propertySelector, GreaterThanFilter<T>? filter) 
        where T : struct
    {
        if (filter is null || filter.Value is null)
        {
            return this;
        }
        
        var propertyRef = propertySelector.Body;
        var parameter = propertySelector.Parameters[0];
        var actualValue = Expression.Constant(filter.Value);

        var res =Expression.Lambda<Func<TModel, bool>>(
            Expression.GreaterThanOrEqual(propertyRef, actualValue),
            parameter);

        _expressions.Add(res);
        
        return this;
    }
    
    public ComplexExpressionBuilder<TModel> Add<T>(Expression<Func<TModel, T>> propertySelector, LessThenFilter<T>? filter) 
        where T : struct
    {
        if (filter is null || filter.Value is null)
        {
            return this;
        }
        
        var propertyRef = propertySelector.Body;
        var parameter = propertySelector.Parameters[0];
        var actualValue = Expression.Constant(filter.Value);

        var res =Expression.Lambda<Func<TModel, bool>>(
            Expression.LessThanOrEqual(propertyRef, actualValue),
            parameter);

        _expressions.Add(res);
        
        return this;
    }
    
    public Expression<Func<TModel, bool>>[] ToArray()
    {
        return _expressions.ToArray();
    }
    
    public static ComplexExpressionBuilder<TModel> Create()
    {
        return new ComplexExpressionBuilder<TModel>();
    }
}