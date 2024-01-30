using System.Linq.Expressions;
using Stock.Application.Interfaces.Expressions;
using Stock.Application.Interfaces.Expressions.Models;

namespace Stock.Infrastructure.Expressions;

public class ExpressionBuilder<TModel> : IExpressionBuilder<TModel> where TModel : class
{
    private readonly List<Expression<Func<TModel, bool>>> _expressions = new();

    public void Add(Expression<Func<TModel, bool>> condition)
    {
        _expressions.Add(condition);
    }

    public void Reset()
    {
        _expressions.Clear();
    }

    public ExpressionBuildResult<TModel> Build()
    {
        return new ExpressionBuildResult<TModel>(_expressions.Count, _expressions.ToArray());
    }
}