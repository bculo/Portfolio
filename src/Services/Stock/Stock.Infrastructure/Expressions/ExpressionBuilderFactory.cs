using Stock.Application.Interfaces.Expressions;

namespace Stock.Infrastructure.Expressions;

public class ExpressionBuilderFactory : IExpressionBuilderFactory
{
    public IExpressionBuilder<TModel> Create<TModel>() where TModel : class, new()
    {
        return new ExpressionBuilder<TModel>();
    }
}