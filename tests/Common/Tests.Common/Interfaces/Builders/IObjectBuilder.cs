using Bogus;

namespace Tests.Common.Interfaces.Builders;

public interface IObjectBuilder<out TType> where TType : class
{
    public abstract TType Build();
}