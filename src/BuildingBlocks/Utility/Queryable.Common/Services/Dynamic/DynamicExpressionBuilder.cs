using System.Linq.Expressions;
using System.Reflection;
using Queryable.Common.Models;

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
        var constantRef = GetSelector(filter.Value, propertySelector);

        var res = Expression.Lambda<Func<TModel, bool>>(
            Expression.Equal(propertySelectorRef, constantRef),
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
        var constantRef = GetSelector(filter.Value, propertySelector);
        
        var res =Expression.Lambda<Func<TModel, bool>>(
            Expression.Call(propertyRef, _containsMethod, constantRef),
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
        var constantRef = GetSelector(filter.Value, propertySelector);

        var res =Expression.Lambda<Func<TModel, bool>>(
            Expression.GreaterThanOrEqual(propertyRef, constantRef),
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
        var constantRef = GetSelector(filter.Value, propertySelector);

        var res =Expression.Lambda<Func<TModel, bool>>(
            Expression.LessThanOrEqual(propertyRef, constantRef),
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
    
    private Expression GetSelector<TProperty>(object value, Expression<Func<TModel, TProperty>> propertySelector)
    {
        var propertyInfo = GetPropertyInfo(propertySelector);
        return value switch
        {
            int => GetTypedSelector<int>(value, propertyInfo),
            float => GetTypedSelector<float>(value, propertyInfo),
            double => GetTypedSelector<double>(value, propertyInfo),
            long => GetTypedSelector<long>(value, propertyInfo),
            DateTime => GetTypedSelector<DateTime>(value, propertyInfo),
            bool => GetTypedSelector<bool>(value, propertyInfo),
            decimal => GetTypedSelector<decimal>(value, propertyInfo),
            char => GetTypedSelector<char>(value, propertyInfo),
            byte => GetTypedSelector<byte>(value, propertyInfo),
            short => GetTypedSelector<short>(value, propertyInfo),
            ushort => GetTypedSelector<ushort>(value, propertyInfo),
            uint => GetTypedSelector<uint>(value, propertyInfo),
            ulong => GetTypedSelector<ulong>(value, propertyInfo),
            string => GetStringSelector(value),
            _ => null
        };
    }
    
    private static Expression GetStringSelector(object value)
    {
        Expression<Func<string>> valueSelector = () => (string)value;
        return valueSelector.Body;
    }
    
    private static PropertyInfo GetPropertyInfo<TIn, TOut>(Expression<Func<TIn, TOut>> expression)
    {
        var memberExp = expression.Body as MemberExpression;
        return memberExp?.Member as PropertyInfo;
    }
    
    private static Expression GetTypedSelector<TProperty>(object value, PropertyInfo pi)
    {
        var propIsNullable = Nullable.GetUnderlyingType(pi.PropertyType) != null;
        Expression<Func<object>> valueSelector = () => value;
        Expression expr = propIsNullable 
            ? Expression.Convert(valueSelector.Body, typeof(TProperty?)) 
            : Expression.Convert(valueSelector.Body, typeof(TProperty));
        return expr;
    }
}