using System.Linq.Expressions;

namespace Queryable.Common.Services.Classic;

public class ExpressionBuilder<TModel> where TModel : class
{
    protected ExpressionBuilder() {} 

    private readonly List<Expression<Func<TModel, bool>>> _expressions = new();

    public ExpressionBuilder<TModel> Add(Expression<Func<TModel, bool>> condition, bool shouldAdd)
    {
        if (shouldAdd)
        {
            _expressions.Add(condition);
        }

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