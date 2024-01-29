using System.Linq.Expressions;

namespace Stock.Infrastructure.Persistence;

public class ParameterReplaceVisitor : ExpressionVisitor
{
   private readonly ParameterExpression _from;
   private readonly ParameterExpression _to;
   
   public ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to)
   {
       _from = from;
       _to = to;
   }
   protected override Expression VisitParameter(ParameterExpression node)
   {
       return node == _from ? _to : base.VisitParameter(node);
   }
}