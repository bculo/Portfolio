using System.Linq.Expressions;
using System.Reflection;
using Queryable.Common.Models;
using Queryable.Common.Utilities;

namespace Queryable.Common.Services.Dynamic;

public class DynamicExpressionBuilder<TModel>
{
    private readonly List<Expression<Func<TModel, bool>>> _expressions = new();
    
    private readonly MethodInfo _containsMethod 
        = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
    
    public DynamicExpressionBuilder<TModel> Add<T>(
        Expression<Func<TModel, T>> propertySelector, 
        EqualFilter<T>? filter)
    {
        if (filter is null || filter.Value is null)
        {
            return this;
        }
        
        var propertySelectorRef = propertySelector.Body;
        var parameter = propertySelector.Parameters[0];
        var value = ExpressionUtilities.GetSelector(propertySelector, filter.Value);

        var res = Expression.Lambda<Func<TModel, bool>>(
            Expression.Equal(propertySelectorRef, value),
            parameter);

        _expressions.Add(res);
        
        return this;
    }
    
    public DynamicExpressionBuilder<TModel> Add(
        Expression<Func<TModel, string>> propertySelector, 
        ContainFilter? filter)
    {
        if (filter is null || string.IsNullOrEmpty(filter.Value))
        {
            return this;
        }
        
        var propertyRef = propertySelector.Body;
        var parameter = propertySelector.Parameters[0];
        var value = ExpressionUtilities.GetSelector(propertySelector, filter.Value);
        
        var res =Expression.Lambda<Func<TModel, bool>>(
            Expression.Call(propertyRef, _containsMethod, value),
            parameter);

        _expressions.Add(res);
        
        return this;
    }
    
    public DynamicExpressionBuilder<TModel> Add<T>(
        Expression<Func<TModel, T>> propertySelector, 
        GreaterThanFilter<T>? filter) 
        where T : struct
    {
        if (filter is null)
        {
            return this;
        }
        
        var propertyRef = propertySelector.Body;
        var parameter = propertySelector.Parameters[0];
        var value = ExpressionUtilities.GetSelector(propertySelector, filter.Value);

        var res =Expression.Lambda<Func<TModel, bool>>(
            Expression.GreaterThanOrEqual(propertyRef, value),
            parameter);

        _expressions.Add(res);
        
        return this;
    }
    
    public DynamicExpressionBuilder<TModel> Add<T>(
        Expression<Func<TModel, T>> propertySelector, 
        LessThenFilter<T>? filter) 
        where T : struct
    {
        if (filter is null)
        {
            return this;
        }
        
        var propertyRef = propertySelector.Body;
        var parameter = propertySelector.Parameters[0];
        var value = ExpressionUtilities.GetSelector(propertySelector, filter.Value);

        var res = Expression.Lambda<Func<TModel, bool>>(
            Expression.LessThanOrEqual(propertyRef, value),
            parameter);

        _expressions.Add(res);
        
        return this;
    }
    
    public Expression<Func<TModel, bool>>[] Build()
    {
        return _expressions.ToArray();
    }
    
    public static DynamicExpressionBuilder<TModel> Create()
    {
        return new DynamicExpressionBuilder<TModel>();
    }
}