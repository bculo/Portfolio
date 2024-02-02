using System.Linq.Expressions;
using Queryable.Common.Models;

namespace Queryable.Common.Services.Classic;

public class ExpressionBuilder<TModel> where TModel : class
{
    private ExpressionBuilder() {} 

    private readonly List<Expression<Func<TModel, bool>>> _expressions = new();

    public ExpressionBuilder<TModel> Add(Expression<Func<TModel, bool>> condition, bool shouldAdd)
    {
        if (shouldAdd)
        {
            _expressions.Add(condition);
        }

        return this;
    }
    
    
    public ExpressionBuilder<TModel> Add<T>(Expression<Func<TModel, bool>> condition, EqualFilter<T>? filter)
    {
        if (filter is null || filter.Value is null)
        {
            return this;
        }
        
        _expressions.Add(condition);
        return this;
    }
    
    public ExpressionBuilder<TModel> Add(Expression<Func<TModel, bool>> condition, ContainFilter? filter)
    {
        if (filter is null || string.IsNullOrEmpty(filter.Value))
        {
            return this;
        }
        
        _expressions.Add(condition);
        return this;
    }
    
    public ExpressionBuilder<TModel> Add<T>(Expression<Func<TModel, bool>> condition, GreaterThanFilter<T>? filter)
        where T : struct
    {
        if (filter is null)
        {
            return this;
        }
        
        _expressions.Add(condition);
        return this;
    }
    
    public ExpressionBuilder<TModel> Add<T>(Expression<Func<TModel, bool>> condition, LessThenFilter<T>? filter)
        where T : struct
    {
        if (filter is null)
        {
            return this;
        }
        
        _expressions.Add(condition);
        return this;
    }
    
    public Expression<Func<TModel, bool>>[] Build()
    {
        return _expressions.ToArray();
    }

    public static ExpressionBuilder<TModel> Create()
    {
        return new ExpressionBuilder<TModel>();
    }
}