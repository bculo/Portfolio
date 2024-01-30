using System.Linq.Expressions;
using Stock.Application.Interfaces.Expressions.Models;

namespace Stock.Application.Interfaces.Expressions;

public interface IExpressionBuilder<TModel> where TModel : notnull
{
    void Add(Expression<Func<TModel, bool>> condition);
    void Reset();
    ExpressionBuildResult<TModel> Build();
}