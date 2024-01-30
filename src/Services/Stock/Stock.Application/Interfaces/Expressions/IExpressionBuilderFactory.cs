namespace Stock.Application.Interfaces.Expressions;

public interface IExpressionBuilderFactory
{
    IExpressionBuilder<TModel> Create<TModel>() where TModel : class, new();
}