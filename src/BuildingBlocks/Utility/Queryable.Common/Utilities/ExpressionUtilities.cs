using System.Linq.Expressions;
using System.Reflection;

namespace Queryable.Common.Utilities;

public static class ExpressionUtilities
{
    public static Expression GetSelector<TModel>(
        string propertyName,
        object value)
    {
        var propertyInfo = GetPropertyInfo(typeof(TModel), propertyName);
        return GetSelectorForProperty(propertyInfo, value);
    }
    
    public static Expression GetSelector<TModel, TProperty>(
        Expression<Func<TModel, TProperty>> propertySelector,
        object value)
    {
        var propertyInfo = GetPropertyInfo(propertySelector);
        return GetSelectorForProperty(propertyInfo, value);
    }

    private static PropertyInfo GetPropertyInfo(Type baseType, string propertyName)
    {
        string[] parts = propertyName.Split('.');
        return (parts.Length > 1)
            ? GetPropertyInfo(baseType.GetProperty(
                    parts[0])?.PropertyType, 
                parts.Skip(1).Aggregate((a, i) => a + "." + i))
            : baseType.GetProperty(propertyName);
    }
    
    private static PropertyInfo GetPropertyInfo<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
    {
        var memberExp = expression.Body as MemberExpression;
        var propertyInfo = memberExp?.Member as PropertyInfo;
        if (propertyInfo is null)
        {
            throw new ArgumentNullException(nameof(propertyInfo));
        }

        return propertyInfo;
    }
    
    private static Expression GetSelectorForProperty(PropertyInfo propertyInfo, object value)
    {
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
    
    private static Expression GetTypedSelector<TProperty>(object value, PropertyInfo propertyInfo)
    {
        var propIsNullable = Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null;
        Expression<Func<object>> valueSelector = () => value;
        Expression expr = propIsNullable 
            ? Expression.Convert(valueSelector.Body, typeof(TProperty?)) 
            : Expression.Convert(valueSelector.Body, typeof(TProperty));
        return expr;
    }
}