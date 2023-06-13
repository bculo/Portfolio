using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Stock.Core.Exceptions;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Stock.ArhitectureTests
{
    public class ArhitectureTests
    {
        private static readonly Architecture Architecture = 
            new ArchLoader().LoadAssemblies(typeof(StockCoreException).Assembly).Build();

        private readonly IObjectProvider<IType> _coreLayer = 
            Types().That().ResideInNamespace("Stock.Core.*", useRegularExpressions: true).As("Core layer");

        private readonly IObjectProvider<IType> _appLayer = 
            Types().That().ResideInNamespace("Stock.Application.*", useRegularExpressions: true).As("Application layer");

        private readonly IObjectProvider<IType> _infraLayer =
            Types().That().ResideInNamespace("Stock.Infrastructure.*", useRegularExpressions: true).As("Infrastructure layer");

        [Fact]
        public void CoreLayer_ShouldNotDependOn_ApplicationLayer()
        {
            Types().That().Are(_coreLayer).Should().NotDependOnAny(_appLayer).Check(Architecture);
        }

        [Fact]
        public void ApplicationLayer_ShouldDependOn_CoreLayer()
        {
            Types().That().Are(_appLayer).Should().DependOnAny(_coreLayer).Check(Architecture);
        }

        [Fact]
        public void InfrastructureLayer_ShouldDependOn_ApplicationLayer()
        {
            Types().That().Are(_infraLayer).Should().DependOnAny(_appLayer).Check(Architecture);
        }

        [Fact]
        public void ApplicationLayer_ShouldNotDependOn_InfrastructureLayer()
        {
            Types().That().Are(_appLayer).Should().NotDependOnAny(_infraLayer).Check(Architecture);
        }
    }
}